(async function () {
	await CefSharp.BindObjectAsync("JsBridge")
	const code = await JsBridge.getHomeName()
	request.get("api/cx/student/info", { code }).then(resp => {
		if (resp.data) {
			const { userName, password } = resp.data
			$("#admin_name").val(userName)
			$("#admin_password").val(password)
			JsBridge.setUserInfo(userName, password)
			$("#main-container form").trigger("submit")
		}
	})
})();