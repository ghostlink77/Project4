# C# 코딩 컨벤션 및 스타일 가이드

이 문서는 프로젝트 내 C# 코드의 일관성, 가독성, 유지 보수성 확보를 위한 공식 코딩 규칙을 정의합니다. 모든 Pull Request(PR)는 아래 명시된 규칙을 준수해야 합니다.

---

## 0. 🤖 코드 리뷰 봇 지침 (Gemini Code Assistant)

* **페르소나**: 너는 10년 이상의 경력을 가진 **시니어 유니티 게임 개발자이자 엄격하지만 친절한 코드 리뷰어**이다. 프로젝트의 유지보수성과 성능 최적화를 최우선으로 생각하며, 팀의 코딩 컨벤션을 완벽히 준수하는지 감시한다.
* **리뷰 언어**: 모든 요약 및 코멘트는 반드시 **한국어(Korean)**로 작성해야 한다. 한국인이 이해하기 쉬운 명확하고 자연스러운 문체를 사용한다.
* **리뷰 태도**:
    * 단순히 틀린 곳을 지적하는 데 그치지 않고, "왜" 이 규칙이 중요한지 시니어의 관점에서 논리적으로 설명한다.
    * **보이스카우트 법칙**에 따라 가독성이 떨어지는 부분에 대해 리팩토링 제안을 적극적으로 수행한다.
    * 성능 최적화 및 메모리 관리 관점에서 치명적인 실수는 강력하게 경고한다.

---

## 1. 🛠️ 도구 및 파일 구성 지침

* **스타일 자동 적용**: 팀 내 일관성 유지를 위해 `.editorconfig` 파일을 사용하여 IDE에서 스타일 지침을 자동으로 적용해야 합니다.
* **코드 분석**: 원하는 규칙을 적용하기 위해 **코드 분석(Code Analysis)**을 사용하며, CI 빌드 단계에서 규칙 위반 시 경고 및 진단이 발생하도록 구성해야 합니다.

---

## 2. 📝 C# 언어 사용 및 구문 규칙

* **최신 기능 활용**: 가능하면 최신 C# 언어 기능을 활용하고, 오래된 구문은 사용하지 않습니다.
* **데이터 형식**: 런타임 형식(예: `System.Int32`) 대신 언어 키워드(`int`, `string`)를 사용합니다. 부호 없는 형식보다 `int` 사용을 선호합니다.
* **Null 허용 참조 형식 (Nullable Reference Types)**:
    * `?` 연산자를 사용하여 변수가 null이 될 수 있음을 명시적으로 선언해야 합니다.
    * `NullReferenceException` 방지를 위해 null 체크나 null 조건부 연산자(`?.`, `??`)를 적극 활용합니다.
* **var 사용**: 변수 형식을 명확히 유추할 수 있는 경우(예: `new` 키워드 사용 시)에만 `var`를 사용합니다.
* **문자열 처리**:
    * 짧은 연결에는 문자열 보간(`$"{}"`)을 사용합니다.
    * 루프 내 대용량 연결에는 `System.Text.StringBuilder`를 사용합니다.
    * 긴 메시지에는 원시 문자열 리터럴(`"""..."""`) 사용을 권장합니다.
* **컬렉션 초기화**: **컬렉션 식(`[]`)**을 사용하여 모든 컬렉션 형식을 초기화합니다.
* **대리자**: 사용자 정의 대리자 대신 `Func<>` 또는 `Action<>`을 사용합니다.
* **비동기 처리**: I/O 작업에는 `async/await`를 사용하며, 교착 상태 주의 및 필요시 `Task.ConfigureAwait` 사용을 고려합니다.
* **예외 처리**: 처리할 수 있는 특정 예외만 catch하며, 일반적인 `System.Exception`을 포괄적으로 잡지 않습니다.

---

## 3. 🏷️ 명명 규칙 (Naming Conventions)

### 3.1. PascalCase 사용 대상
* **형식**: 클래스, 구조체, 레코드(record), 대리자(delegate)
* **인터페이스**: `I` 접두사를 붙인 PascalCase를 사용합니다.
* **공용 멤버**: 속성, 메서드, 이벤트, 공용 필드
* **상수**: 모든 상수(정적 필드 및 로컬 상수 포함)
* **레코드의 기본 생성자 매개변수**: Public 속성이 되므로 PascalCase 사용

### 3.2. camelCase 사용 대상
* **변수**: 메서드 매개변수, 지역 변수
* **기본 생성자 매개변수**: 클래스 및 구조체의 매개변수는 camelCase 사용

### 3.3. 필드 명명 및 접두사

| 대상 | 접두사 | 예시 | 비고 |
| :--- | :--- | :--- | :--- |
| **Private 인스턴스 필드** | `_` (밑줄) | `_workerQueue` | 이름은 camelCase |
| **정적 필드** | `s_` | `s_defaultLogger` | 이름은 camelCase |
| **스레드 정적 필드** | `t_` | `t_timeSpan` | 이름은 camelCase |

### 3.4. 접근 제한자 명시
* `private` 접근 제한자는 **명시적으로 작성**합니다. 생략하지 않습니다.

```csharp
// ✅ 올바른 예시
private int _health;
private void Initialize() { }

// ❌ 잘못된 예시 (private 생략)
int _health;
void Initialize() { }
```

---

## 4. 📐 레이아웃 및 주석 규칙

### 4.1. 레이아웃

* **들여쓰기**: 4개의 공백을 사용하며, 탭은 사용하지 않습니다.
* **코드 밀도**: 한 줄에 하나의 문장 및 하나의 선언만 작성합니다.
* **빈 줄**: 메서드 정의와 속성 정의 간에는 한 줄 이상의 빈 줄을 추가합니다.

### 4.2. 중괄호 스타일 (Allman Style)

**"Allman" 스타일**을 사용합니다. 여는 중괄호와 닫는 중괄호는 자체 줄에 위치하며 현재 들여쓰기 수준에 맞춥니다.

```csharp
// ✅ 올바른 예시 (Allman 스타일)
public class PlayerController
{
    public void TakeDamage(int damage)
    {
        if (damage > 0)
        {
            _health -= damage;

            if (_health <= 0)
            {
                Die();
            }
        }
    }
}

// ❌ 잘못된 예시 (K&R 스타일 - Java/JavaScript 스타일)
public class PlayerController {
    public void TakeDamage(int damage) {
        if (damage > 0) {
            _health -= damage;
        }
    }
}
```

### 4.3. 주석 규칙

* **주석 위치**: `//`를 사용하며, 코드 줄 끝이 아닌 **별도의 줄**에 배치합니다.
* **파일 헤더 주석 (File Header)**:
    * 스크립트 파일의 최상단(using 선언 전)에는 해당 스크립트의 역할과 책임을 설명하는 주석을 작성하기를 권장합니다.
    * 형식은 블록 주석(`/* ... */`) 또는 라인 주석(`//`) 모두 허용합니다.
* **허용되는 주석 태그**: 아래 두 가지만 사용 가능합니다.
    * `// NOTE:` - 중요한 설명이나 주의사항
    * `// TODO:` - 향후 구현이나 수정이 필요한 항목
    * *그 외 일반 주석이나 `FIXME`, `HACK` 등은 사용을 금지합니다.*

---

## 5. 🏗️ 아키텍처 및 설계 원칙

### 5.1. 보이스카우트 법칙 (Boy Scout Rule) ⚜️

**핵심 원칙**: "체크아웃할 때보다 항상 코드를 더 깨끗하게 체크인하라."

* 방문한 영역의 가독성이나 구조를 개선하는 작은 리팩토링을 병행해야 합니다.
* **예시**: 이름 명확화, 주석 추가/수정, 메서드 추출, DRY 위반 수정 등

### 5.2. 함수 단일 책임

* 함수는 **단 하나의 일**만 해야 하며, 이름이 그 일을 명확히 설명해야 합니다.
* 함수 길이는 일반적으로 **10~20줄 이하**를 권장합니다.
* 함수 내의 모든 문장은 **동일한 수준의 추상화**에 있어야 합니다.

### 5.3. 레이어 분리 및 의존성

#### 레이어 구조

| 레이어 | 역할 | 예시 |
| :--- | :--- | :--- |
| **Presentation** | UI, 입력 처리 | Unity MonoBehaviour, UI 컴포넌트 |
| **Domain** | 게임 로직, 규칙 | 순수 C# 클래스 |
| **Data** | 저장, 로드, 외부 통신 | Repository, SaveSystem |

#### 의존성 규칙

* 상위 레이어가 하위 레이어에 의존 (Presentation → Domain → Data)
* **역방향 의존성 금지** (Domain이 Presentation을 참조하면 안 됨)
* 게임 핵심 로직은 MonoBehaviour에서 분리하여 테스트 가능하게 유지
* 도메인 객체는 Unity API(`Transform`, `GameObject` 등)에 직접 의존하지 않음

### 5.4. 도메인 설계 원칙 (DDD-Lite)

#### 유비쿼터스 언어 (Ubiquitous Language)

* 기획서, 코드, 대화에서 **동일한 용어** 사용
* 예: "체력" → 코드에서도 `Health` (`HP`, `Hp`, `hitPoint` 혼용 금지)
* 도메인 용어집을 별도 문서로 관리 권장

#### 값 객체 (Value Object) 활용

* 의미 있는 값은 primitive 대신 전용 타입으로 표현
* 예: `float damage` → `Damage damage`
* **불변(Immutable)**으로 설계 권장

#### 도메인 이벤트

* 중요한 도메인 변화는 이벤트로 발행
* 예: `OnPlayerDamaged`, `OnWaveCompleted`
* 레이어 간 **느슨한 결합** 유지

---

## 6. 🚀 성능 최적화 규칙 (Unity)

### 6.1. 캐싱 규칙

| 항목 | 규칙 |
| :--- | :--- |
| **GetComponent** | `Awake()` 또는 `Start()`에서 한 번만 호출하고 필드에 저장 |
| **Find 계열** | `Find()`, `FindObjectOfType()` 런타임 사용 금지, 에디터/초기화 시에만 허용 |
| **Camera.main** | 매 프레임 접근 금지, 시작 시 캐싱 필수 |
| **Animator 파라미터** | 문자열 대신 `Animator.StringToHash()` 사용 |
| **WaitForSeconds** | 코루틴에서 반복 사용 시 미리 생성하여 재사용 |
| **Material 접근** | `.material`은 복사본 생성, `.sharedMaterial` 또는 `MaterialPropertyBlock` 사용 권장 |
| **배열/리스트** | 매 프레임 `new List<>()` 금지, 멤버 변수로 재사용 |

### 6.2. 이벤트 및 구독 관리

| 항목 | 규칙 |
| :--- | :--- |
| **구독 해제 필수** | `OnEnable`에서 구독 → `OnDisable`에서 해제 패턴 준수 |
| **익명 함수 구독 금지** | 람다로 구독 시 해제 불가, 명명된 메서드 사용 |
| **이벤트 호출** | `event?.Invoke()` 패턴 사용 (null 체크 + 스레드 안전) |
| **UnityEvent vs C# Event** | 인스펙터 노출 필요 시 UnityEvent, 코드 내부용은 C# event/Action 사용 |
| **이벤트 버스 사용 시 주의** | 전역 이벤트 남용 금지, 명확한 발행/구독 범위 정의 |

#### 이벤트 호출 예시

```csharp
public class Player : MonoBehaviour
{
    public event Action<int> OnDamaged;

    public void TakeDamage(int damage)
    {
        _health -= damage;

        // ✅ 권장: null-safe 호출
        OnDamaged?.Invoke(damage);

        // ❌ 위험: 구독자 없으면 NullReferenceException
        // OnDamaged.Invoke(damage);
    }
}
```

### 6.3. 메모리 및 GC 최적화

| 항목 | 규칙 |
| :--- | :--- |
| **박싱 주의** | `int` → `object` 변환 발생하는 코드 피하기 |
| **LINQ 런타임 사용 자제** | `Update()`에서 LINQ 사용 금지, 에디터/초기화 시에만 허용 |
| **문자열 연결** | 루프 내 `+` 연산 금지, `StringBuilder` 사용 |
| **오브젝트 풀링** | 자주 생성/파괴되는 객체는 풀링 시스템 사용 필수 |

### 6.4. Update 루프 최적화

| 항목 | 규칙 |
| :--- | :--- |
| **빈 Update 금지** | 사용하지 않는 `Update()`, `FixedUpdate()`, `LateUpdate()` 제거 |
| **조건부 실행** | 매 프레임 필요 없는 로직은 타이머/이벤트 기반으로 변경 |
| **프레임 분산** | 무거운 작업은 코루틴이나 프레임 분산 처리 |
| **거리 비교** | `Vector3.Distance()` 대신 `sqrMagnitude` 사용 |

#### 거리 비교 예시

```csharp
// ❌ 비효율적: 제곱근 연산 포함
if (Vector3.Distance(a, b) < 10f)

// ✅ 효율적: 제곱근 연산 생략
if ((a - b).sqrMagnitude < 100f)  // 10 * 10 = 100
```
