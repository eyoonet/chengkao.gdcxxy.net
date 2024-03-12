// ==UserScript==
// @name         New Userscript
// @namespace    http://tampermonkey.net/
// @version      0.1
// @description  try to take over the world!
// @author       You
// @match        https://chengkao.gdcxxy.net/gdcx/jp_wiki_study.php*
// @icon         https://www.google.com/s2/favicons?sz=64&domain=gdcxxy.net
// @grant        none
// ==/UserScript==

(function () {
    'use strict';
    var start = true
    $("#player-container-id").remove()
    const [hours, minutes, seconds] = $(".si-control-play +div +div:first").text().split("/").pop().replace(")", "").split(":").map(Number)
    const totalSeconds = hours * 3600 + minutes * 60 + seconds;

    window.vtt = function () {
        try {
            var captcha = new TencentCaptcha('190940375', callback_vtt, {});
            captcha.show();
        } catch (error) {
            loadErrorCallback_vtt();
        }
        setInterval(() => {
            $("#tcaptcha_transform").css("top", "-65px")
            $("#tcaptcha_transform").css("left", "-15px")
        }, 1000)

    }

    window.callback_vtt = function (res) {
        if (res.ret === 0) {
            var ticket = res.ticket;
            var randstr = res.randstr;
            var TypeId = document.getElementById("TypeId").value
            var jjwikiid = document.getElementById("jjwikiid").value;
            $.post('jp_study_vtt.action.php', { ticket: ticket, randstr: randstr, TypeId: TypeId, jjwikiid: jjwikiid }, function (e) {
                if (e.code == 200) {
                    $('.loggn').val('0');
                    start = true
                    // face_ewm();
                } else {
                    layer.open({
                        title: '提示信息',
                        content: e.msg,
                        offset: '10px'
                    });
                    $('.loggns').val('0');
                    window.location.reload()
                }
            }, 'json')
        } else {
            $('.loggns').val('0');
        }
    }
    window.callback = function (res) {
        if (res.ret === 0) {
            var ticket = res.ticket;
            var randstr = res.randstr;
            var TypeId = document.getElementById("TypeId").value
            var jjwikiid = document.getElementById("jjwikiid").value;
            $.post('jp_study.action.php', { ticket: ticket, randstr: randstr, TypeId: TypeId, jjwikiid: jjwikiid }, function (e) {
                if (e.code == 200) {
                    // face_ewm();
                    start = true;
                } else {
                    layer.open({
                        title: '提示信息',
                        content: e.msg,
                        offset: '10px'
                    });
                    var cur_jindu = document.getElementById("cur_jindu").value;
                    player.currentTime(cur_jindu);
                }
            }, 'json')
        }
    }
    console.log(totalSeconds)
    var t = setInterval(() => {
        var cur_jindu = document.getElementById("max_jindu").value;
        if (start) {
            if (parseInt(cur_jindu) + 5 < parseInt(totalSeconds)) {
                handle()
            } else {
                var next = $(".si-control-play").parent().next().find("a").attr("href")
                start = false
                if (parseInt(cur_jindu) != totalSeconds) {
                    document.getElementById("max_jindu").value = 999999 - 5;
                    handle(() => {
                        next && (top.location.href = next)
                    })
                } else {
                    next && (top.location.href = next)
                }
            }
        }
    }, 5200)


    function handle(callback = null) {
        var TypeId = document.getElementById("TypeId").value;
        var jjwikiid = document.getElementById("jjwikiid").value;
        var cur_jindu = parseInt(document.getElementById("max_jindu").value) + 5;
        var userlog_id = document.getElementById("userlog_id").value;
        let sign = md5(`${TypeId}|${jjwikiid}|${cur_jindu}|${userlog_id}`)
        let time = $('[name="load_time"]').val()
        $.get(
            "learn_jjsp.action.php",
            {
                TypeId: TypeId,
                jjwikiid: jjwikiid,
                cur_jindu: cur_jindu,
                userlog_id: userlog_id,
                sign: sign,
                time: time
            },

            function (c) {
                //debugger
                if (c == 'video_img_yz') {
                    start = false
                    face_ewm();
                    return;
                } else if (c == 'video_yz') {
                    CaptchaId_yz();
                    return;
                } else if (c == 'video_vtt') {
                    start = false
                    $('.loggn').val('1');
                    $('.loggns').val('1');
                    vtt();
                    return;
                } else if (c == 'other') {
                    //alert('系统超时或用户已在其他地方登录，请重新登录！注意：用户同时打开多个学习页面也将导致系统自动退出！');
                    top.location.href = 'login2.php';
                } else if (c == '888888') {
                    alert('学习进度异常，请重新开始学习！');
                    window.location.reload();
                } else if (c == 'same') {

                    //alert('为保证学习效果，不允许同时学习多个课程！系统已自动退出，请重新登录！');
                    top.location.href = 'login2.php';
                } else if (c == 'over10') {

                    alert('您今天已经新学了10个课时（450分钟），请明天再来学习新课程！为保证学习效果，每天最多新学10个课时。');
                    //top.location.href = 'mycourse.php';
                    var userName = $("#con_one_2 ul:nth-child(2) > li:nth-child(2) > input[type=text]").val();
                    request.post("api/cx/student/status/over10", { userName }).then(resp => {
                        return (top.location.href = 'login2.php');
                    })

                } else if (c == 'updating') {

                    alert('本课程正在更新中，请明天再来学习该课程。课程更新不会影响您已有的学习进度。现在您可以先学习其他课程。');
                    top.location.href = 'mycourse.php';
                } else if (c == 'is_pass') {

                    //alert('此用户当前无法登录。如有疑问，请联系您的班主任老师。');
                    top.location.href = 'login2.php';
                } else {
                    document.getElementById("max_jindu").value = c;
                    $("#main-container .block-content-full > div:nth-child(1)").html(new Date().toLocaleTimeString() + "--->" + c + " / " + totalSeconds)
                    if (callback) callback()
                    if (c < 120) {
                        document.getElementById('mj_m_01').innerHTML = parseInt(c) + ' 秒';
                        document.getElementById('mj_m_02').innerHTML = parseInt(c) + ' 秒';
                    } else {
                        document.getElementById('mj_m_01').innerHTML = parseInt(c / 60) + ' 分钟';
                        document.getElementById('mj_m_02').innerHTML = parseInt(c / 60) + ' 分钟';
                    }
                }
                return;
            });
    }
})();