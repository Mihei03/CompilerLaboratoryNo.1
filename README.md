<!DOCTYPE html>
<html>
<body>
  <h1>Компилятор</h1>
  <p>Разработка текстового редактора с функциями языкового процессора.</p>

  <h2>Оглавление</h2>
  <ul>
    <li><a href="#lab1">Лабораторная работа №1: Разработка пользовательского интерфейса (GUI) для языкового процессора</a></li>
    <li><a href="#lab2">Лабораторная работа №2: Разработка лексического анализатора (сканера)</a></li>
    <li><a href="#lab3">Лабораторная работа №3: Разработка синтаксического анализатора (парсера)</a></li>
    <li><a href="#lab4">Лабораторная работа №4: Нейтрализация ошибок (метод Айронса)</a></li>
    <li><a href="#lab5">Лабораторная работа №5: Включение семантики в анализатор. Создание внутренней формы представления программы</a></li>
    <li><a href="#lab6">Лабораторная работа №6: Реализация алгоритма поиска подстрок с помощью регулярных выражений</a></li>
    <li><a href="#lab7">Лабораторная работа №7: Реализация метода рекурсивного спуска для синтаксического анализа</a></li>
  </ul>

  <h2 id="lab1">Лабораторная работа №1: Разработка пользовательского интерфейса (GUI) для языкового процессора</h2>
  <p>Тема: разработка текстового редактора с возможностью дальнейшего расширения функционала до языкового процессора.</p>

  <p>Цель работы: разработать приложение с графическим интерфейсом пользователя, способное редактировать текстовые данные. Это приложение будет базой для будущего расширения функционала в виде языкового процессора.</p>

  <p>Язык реализации: C#, WPF.</p>

  <h3>Интерфейс текстового редактора</h3>
  <p>Главное окно программы</p>
  <img src="bin/Debug/net7.0-windows/Readme/Example.jpg" alt="Рабочее окно текстового редактора с обозначенными элементами.">
  
  <h3>Получившийся текстовый редактор имеет следующие элементы:</h3>
  <ol>
    <li value="1">Заголовок окна.</li>  
    <p>Содержит информацию о названии открытого файла, полного пути к нему, а также о том, сохранен ли он на текущий момент (наличие символа звездочки справа от названия означает наличие несохраненных изменений).</p>
    <li value="2">Меню.</li>  
    <table>
      <tr>
        <th>Пункт меню</th>
        <th>Подпункты</th>
      </tr>
      <tr>
        <td>Файл</td>
        <td><img src="bin/Debug/net7.0-windows/Readme/File.png" alt="File"></td>
      </tr>
      <tr>
        <td>Правка</td>
        <td><img src="bin/Debug/net7.0-windows/Readme/Edit.png" alt="Edit"></td>
      </tr>
      <tr>
        <td>Текст</td>
        <td><img src="bin/Debug/net7.0-windows/Readme/Text.png" alt="Text"></td>
      </tr>
      <tr>
        <td>Пуск</td>
        <td>Отсутствует</td>
      </tr>
      <tr>
        <td>Справка</td>
        <td><img src="bin/Debug/net7.0-windows/Readme/Reference.png" alt="Reference"></td>
      </tr>
    </table>
    <li value="3">Панель инструментов</li>
	<img src="bin/Debug/net7.0-windows/Readme/Toolbar.png" alt="Toolbar">
      <ul>
        <li>Создать</li>
        <li>Открыть</li>
        <li>Сохранить</li>
        <li>Отменить</li>
        <li>Повторить</li>
        <li>Копировать</li>
        <li>Вырезать</li>
        <li>Вставить</li>
        <li>Изменить размер текста</li>
      </ul>
    <li value="4">Область редактирования</li>
    <p>Поддерживаются следующие функции:</p>
      <ul>
        <li>Изменение размера текста</li>
        <li>Открытие файла при перетаскивании его в окно программы</li>
      </ul>
    <li value="5">Область отображения результатов</li>
    <p>В область отображения результатов выводятся сообщения и результаты работы языкового процессора.</p>
    <p>Поддерживаются следующие функции:</p>
      <ul>
        <li>Изменение размера текста</li>
        <li>Отображение ошибок в виде таблицы</li>
      </ul>
  </ol>
    <h3>Справочная система</h3>
    <p>Разделы справочной системы открываются как HTML-документы в браузере.</p>
    <table>
      <tr>
        <th>Раздел</th>
        <th>Изображение</th>
      </tr>
      <tr>
        <td>Вызов справки</td>
        <td><img src="bin/Debug/net7.0-windows/Readme/Information.png" alt="Information"></td>
      </tr>
      <tr>
        <td>О программе</td>
        <td><img src="bin/Debug/net7.0-windows/Readme/About.png" alt="About"></td>
      </tr>
    </table>
    <h3>Вывод сообщений</h3>
    <table>
      <tr>
        <th>Сообщение</th>
        <th>Описание</th>
      </tr>
      <tr>
        <td>Закрытие окна программы</td>
        <td>Появляется при закрытии программы нажатием крестика или комбинацией клавиш при наличии несохраненных изменений</td>
      </tr>
      <tr>
        <td>Сохранение изменений</td>
        <td>Появляется при попытке открыть существующий файл или создать новый при наличии несохраненных изменений	Сохранение изменений</td>
      </tr>
    </table>
   
  <h2 id="lab2">Лабораторная работа №2: Разработка лексического анализатора (сканера)</h2>
  <p>Тема: разработка лексического анализатора (сканера).</p>

  <p>Цель работы: изучить назначение лексического анализатора. Спроектировать алгоритм и выполнить программную реализацию сканера.</p>
  
  <table>
    <tr>
      <th>№</th>
      <th>Тема</th>
      <th>Пример верной строки</th>
      <th>Справка</th>
    </tr>
    <tr>
      <td>3</td>
      <td>Объявление комплексного числа с инициализацией на языке Python</td>
      <td>z3 = complex(0, 2.5)</td>
      <td><a href="https://stepik.org/lesson/360942/step/11">Справка</a></td>
    </tr>
  </table>

  <h3>В соответствии с вариантом задания необходимо:</h3>
  <ol>
    <li>Спроектировать диаграмму состояний сканера.</li>
    <li>Разработать лексический анализатор, позволяющий выделить в тексте лексемы, иные символы считать недопустимыми (выводить ошибку).</li>
    <li>Встроить сканер в ранее разработанный интерфейс текстового редактора. Учесть, что текст для разбора может состоять из множества строк.</li>
  </ol>

  <p>Входные данные: строка (текст программного кода).</p>

  <p>Выходные данные: последовательность условных кодов, описывающих структуру разбираемого текста с указанием места положения и типа.</p>

  <h3>Примеры допустимых строк</h3>
  <pre>
    DECLARE product_price CONSTANT INTEGER = +150;
    DECLARE total_amount CONSTANT INTEGER := -150;
    DECLARE productPrice CONSTANT INTEGER := +150;
    DECLARE expense_1_amount CONSTANT INTEGER := -50;
    DECLARE product_price CONSTANT INTEGER := -150; DECLARE total_2 CONSTANT INTEGER := 50;
    DECLARE productPrice3 CONSTANT INTEGER := 150; DECLARE expense_amount_4 CONSTANT INTEGER := -50;
  </pre>

  <h3>Диаграмма состояний сканера</h3>
  <img src="" alt="Диаграмма состояний сканера">

  <h3>Тестовые примеры</h3>
  <h4>Тест №1. Пример, показывающий все возможные лексемы, которые могут быть найдены лексическим анализатором.</h4>
  <img src="" alt="Диаграмма состояний сканера">

  <h4>Тест №2. Сложный пример.</h4>
  <img src="" alt="Диаграмма состояний сканера">

  <h4>Тест №3. Сложный пример.</h4>
  <img src="" alt="Диаграмма состояний сканера">

  
</body>
</html>
