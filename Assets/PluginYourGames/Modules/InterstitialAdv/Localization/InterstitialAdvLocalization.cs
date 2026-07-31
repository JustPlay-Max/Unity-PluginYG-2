namespace YG.Localization
{
    public class InterstitialAdvLocalization
    {
#if RU_YG2
        public const string SecondsPanelObject = "Объект таймера перед показом рекламы. Он будет активироваться и деактивироваться в нужное время.";
#else
        public const string SecondsPanelObject = "The timer object before the ad is shown. It will activate and deactivate at the right time.";
#endif

#if RU_YG2
        public const string SecondObjects = "Массив объектов, которые будут показываться по очереди через секунду. Сколько объектов вы поместите в массив, столько секунд будет отчитываться перед показом рекламы.\n\nНапример, поместите в массив три объекта: певый с текстом '3', второй с текстом '2', третий с текстом '1'.\nВ таком случае произойдёт отчет трёх секунд с показом объектов с цифрами перед рекламой.";
#else
        public const string SecondObjects = "An array of objects that will be displayed in turn in a second. How many objects you put in the array will be reported for as many seconds before the ad is shown.\n\nFor example, put three objects in the array: the left with the text '3', the second with the text '2', the third with the text '1'.\nIn this case, a three-second report will occur showing objects with numbers before advertising.";
#endif
    }
}