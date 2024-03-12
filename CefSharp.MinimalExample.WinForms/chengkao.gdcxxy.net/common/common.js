(async function () {
	await CefSharp.BindObjectAsync("JsBridge")

    window.request = {
        BASE_URL: "https://que.kunzejiaoyu.cn/dev-api/", //本地链接
        get: function (url, params) {
            let _this = this;
            return new Promise(function (resolve, reject) {
                $.ajax({
                    type: "get",
                    data: params,
                    url: _this.BASE_URL + url,//服务器接口
                    dataType: 'json',
                    success: function (data) {
                        resolve(data)
                    },
                    error: function (e) {
                        reject(e)
                    }
                });
            }).catch(function (reason) {
                //有选择性的在此处抛出错误或不抛出
                console.error('catch:', reason);
            });
        },
        post: function (url, params) {
            let _this = this;
            //proxy.close()//关闭代理
            return new Promise(function (resolve, reject) {
                $.ajax({
                    type: "post",
                    data: params,
                    url: _this.BASE_URL + url,//服务器接口
                    dataType: 'json',
                    success: function (data) {
                        // proxy.setProxyIp()//开启代理
                        resolve(data)
                    },
                    error: function (e) {
                        reject(e)
                    }
                });
            }).catch(function (reason) {
                //有选择性的在此处抛出错误或不抛出
                return redirect("/")
                console.error('catch:', reason);
            });
        },
        postByJson: function (url, params) {
            let _this = this;
            //proxy.close()//关闭代理
            return new Promise(function (resolve, reject) {
                $.ajax({
                    type: "post",
                    data: JSON.stringify(params),
                    url: _this.BASE_URL + url,//服务器接口
                    dataType: 'json',
                    contentType: "application/json;charset=UTF-8", //必须有
                    async: false,
                    success: function (data) {
                        //proxy.setProxyIp()//开启代理
                        if (data.code === 200) {
                            resolve(data)
                        } else {
                            reject(data)
                        }
                    },
                    error: function (e) {
                        reject(e)
                    }
                });
            });
        }
    }

})();