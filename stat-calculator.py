# 1레벨부터 8레벨까지 있을 때 스탯 계산기
# 일정한 간격으로 얼마나 증가시킬 것인지 확인할 수 있음

max_level = 8
max_rate = float(input(f"1레벨이 100%라고 가정할 때, 최대 레벨({max_level})에서 1레벨의 몇 %까지 증가시킬 건가요?(%):"))

first_stat = float(input("초기 스탯값을 입력하세요:"))

adder = ((max_rate-100) / (max_level - 1)) / 100
adder *= first_stat
print(f"레벨 당 증가값: {adder}")

for i in range(max_level):
    result = round(first_stat + adder * i, 0)
    print(f"{i+1}번째 스탯: {result}")