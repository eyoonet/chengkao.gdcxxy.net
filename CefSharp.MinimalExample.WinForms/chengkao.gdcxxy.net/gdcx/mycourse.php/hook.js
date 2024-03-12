(function () {

	window.gotoxx = function(is_sp, kcdm) {
		if (is_sp == 1) {
			window.location.href = 'jp_wiki_study.php?kcdm=' + kcdm
		} else {
			layer.open({
				title: '提示信息',
				content: '本课程正在更新中，请稍后再来学习！',
				offset: '10px'
			});
		}
	}


	window.gotolx = function(is_sp, kcdm) {
		if (is_sp == 1) {
			window.location.href = 'studycenter_zy.php?kcdm=' + kcdm
		} else {
			layer.open({
				title: '提示信息',
				content: '本课程正在更新中，请稍后再来学习！',
				offset: '10px'
			});
		}
	}
	window.gotocp = function(is_sp, kcdm) {
		if (is_sp == 1) {
			window.location.href = 'studycenter_cy.php?kcdm=' + kcdm;
		} else {
			layer.open({
				title: '提示信息',
				content: '本课程正在更新中，请稍后再来学习！',
				offset: '10px'
			});
		}
	}

	var $itemBox = $(".itemBox");
	for (let item of $itemBox) {
		var $number = $(item).find(".numbox span:nth-child(2) .text-default")
		var name = $(item).find(".h1:first").text()
		if (parseInt($number.text()) !== 100 && !name.includes("PA990026")) {
			var $button = $(item).find(".butt button:first")
			return $button.trigger("click")
		}
	}
})();