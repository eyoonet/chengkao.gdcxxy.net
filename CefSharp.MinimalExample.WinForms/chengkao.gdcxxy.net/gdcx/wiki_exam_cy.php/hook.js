(function () {
    $(".answer").removeClass("none")
    $("#resolve").show()

    var answer = $("#resolve p").text().replace("正确答案：", "")
    var $label = $(".single_input label")
    var type = $(".single_choice:first").text();

    if (type.includes('单项选择题') || type.includes('判断题')) {
        for (let item of $label) {
            if ($(item).text().includes(answer)) {
                $(item).trigger("click")
            }
        }
        $("#sub_an > li > a").trigger("click")
    }
    //简答题
    if (type.includes('简答题')) {
        $("#allqita").val(answer)
        $("#sub_an > li > a").trigger("click")
    }
    //多选题
    if (type.includes('多项选择题')) {
        var answers = answer.split('')
        for (let ans of answers) {
            for (let item of $label) {
                if ($(item).text().includes(ans)) {
                    $(item).trigger("click")
                }
            }
        }
        $("#sub_an > li > a").trigger("click")
    }


    setTimeout(() => {
        window.location.href = $(".next_choice a:first").attr("href")
    }, 2000)
 

})();