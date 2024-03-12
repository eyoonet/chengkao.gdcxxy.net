(function () {

	window.gotocy = function (TypeId, chapteridlist, is_wccy, is_jddb, cy_bianhao) {
		window.location.href = 'wiki_exam_cy.php?TypeId=' + TypeId + '&chapteridlist=' + chapteridlist + '&cy_bianhao=' + cy_bianhao;
	}


	var $tr = $(".courseBoxtwo tbody > tr:not(.line,.titOnetwo,.titOne)");

	for (let item of $tr) {
		var $button = $(item).find("button:first")
		var status = $(item).find("td:nth-child(2)").text()
		console.log(status)
		
		if (!status.includes('已完成')) {
			return $button.trigger("click")
		}
	}
})();