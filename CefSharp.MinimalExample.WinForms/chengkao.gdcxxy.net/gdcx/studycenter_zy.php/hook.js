(function () {

	window.gotozy = function (TypeId, jjwikiid) {
		window.location.href = 'wiki_exam_zy.php?TypeId=' + TypeId + '&jjwikiid=' + jjwikiid;	
	}

	var $tr = $("tbody > tr:not(.line,.titOnetwo,[name='detail'],.titOne)");

	for (let item of $tr) {
		var $button = $(item).find("button:first")
		var status = $(item).find("td:nth-child(3)").text()
		console.log(status)
		if (!status.includes('已完成')) {
			return $button.trigger("click")
		}
	}
})();