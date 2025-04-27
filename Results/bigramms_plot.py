import matplotlib.pyplot as plt
import numpy as np
import matplotlib


matplotlib.rc('font', family='Verdana')

bigrams = []
real_probability = []
expected_probability = []
with open('Results/bigramms_data.txt', 'r', encoding='utf-8') as file:
    for line in file:
        values = line.replace(',','.').strip().split(' ')
        bigrams.append(values[0])
        real_probability.append(float(values[1]))
        expected_probability.append(float(values[2]))


bigrams = bigrams[:100]
real_probability = real_probability[:100]
expected_probability = expected_probability[:100]

bar_width = 0.35
index = np.arange(len(bigrams))

plt.figure(figsize = (25, 15))
ax = plt.subplot()

bars1 = ax.bar(index - bar_width/2, real_probability, bar_width, label = 'Реальные значения', color = 'skyblue')
bars2 = ax.bar(index + bar_width/2, expected_probability, bar_width, label = 'Ожидаемые значения', color = 'orange')

ax.set_title('Сравнение вероятностей биграмм', fontsize=19)
ax.set_xlabel('Биграммы')
ax.set_ylabel('Вероятности')
ax.set_xticks(index)
ax.set_xticklabels(bigrams, rotation=90, ha='center', va='top', fontsize=9)
for xtick in index:
    ax.axvline(xtick, color='gray', alpha=0.1, linestyle='--')
ax.legend(fontsize=12)

plt.grid(axis='y', alpha=0.75)

plt.tight_layout()

plt.show()


