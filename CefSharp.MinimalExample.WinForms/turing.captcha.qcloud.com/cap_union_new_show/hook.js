(async function () {
	await CefSharp.BindObjectAsync("JsBridge")
	
	var script = document.createElement('script');
	script.setAttribute('type', 'text/javascript');
	script.setAttribute('src', 'https://cdn.bootcss.com/html2canvas/0.5.0-beta4/html2canvas.js');
	script.setAttribute('id', "newScript");
	document.getElementsByTagName('body')[0].appendChild(script);

	script.onload = () => {
		html2canvas($("#tcaptcha_imgarea"), {
			onrendered: function (canvas) {
				var url = canvas.toDataURL("image/png");
				var text = $(".tcaptcha-title:last").text()
				$.post('https://www.chengkaojiaoyu.net/jfbym.php', { image: encodeURIComponent(url), extra: text }, (response) => {
					var resp = JSON.parse(response)
					if (resp.code == 10000) {
						var respData = resp.data
						let [x, y] = respData.data.split(',')
						console.log('res', parseInt(x), parseInt(y))
						setTimeout(() => {
							console.log("执行点击")
							JsBridge.click(parseInt(x), parseInt(y))
						},2000)
					}
				})
			}
		});
	}
})();