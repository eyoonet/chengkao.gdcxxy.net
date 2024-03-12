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
    $("#player-container-id").remove()
    const [hours, minutes, seconds] = $(".si-control-play +div +div:first").text().split("/").pop().replace(")", "").split(":").map(Number)
    const totalSeconds = hours * 3600 + minutes * 60 + seconds;
    debugger
    console.log(totalSeconds)
    var t = setInterval(() => {
        //$.ajax({async: false})
        var shichang = document.getElementById("shichang").value;
        var cur_jindu = document.getElementById("cur_jindu").value;
        if (parseInt(cur_jindu) + 6 < parseInt(totalSeconds)) {
            handle()
        } else {
            clearInterval(t)
            document.getElementById("cur_jindu").value = parseInt(totalSeconds);
            onplayend()
            var next = $(".si-control-play").parent().next().find("a").attr("href")
            top.location.href = next
        }
    }, 5100)
    function handle() {
        var TypeId = document.getElementById("TypeId").value;
        var jjwikiid = document.getElementById("jjwikiid").value;
        var userlog_id = document.getElementById("userlog_id").value;
        var cur_jindu = document.getElementById("cur_jindu").value;
        $.get("learn_jjsp.action.php", {
            TypeId: TypeId, jjwikiid: jjwikiid, cur_jindu: parseInt(cur_jindu) + 6, userlog_id: userlog_id, r: Math.random()
        }, function (c) {
            //debugger
            if (c == 'video_img_yz') {
                clearInterval(t)
                player.pause();
                face_ewm();
                return;
            } else if (c == 'video_yz') {
                CaptchaId_yz();
                return;
            } else if (c == 'video_vtt') {
                clearInterval(t)
                $('.loggn').val('1');
                $('.loggns').val('1');
                player.pause();
                vtt();
                return;
            } else if (c == 'other') {
                //videoPlayer.pause();
                document.getElementById("cur_jindu").value = cur_jindu;
                alert('系统超时或用户已在其他地方登录，请重新登录！注意：用户同时打开多个学习页面也将导致系统自动退出！');
                top.location.href = 'login.php';
            } else if (c == '888888') {
                //videoPlayer.pause();
                document.getElementById("cur_jindu").value = cur_jindu;
                alert('学习进度异常，请重新开始学习！');
                window.location.reload();
            } else if (c == 'same') {
                //videoPlayer.pause();
                document.getElementById("cur_jindu").value = cur_jindu;
                alert('为保证学习效果，不允许同时学习多个课程！系统已自动退出，请重新登录！');
                top.location.href = 'login.php';
            } else if (c == 'over10') {
                //videoPlayer.pause();
                document.getElementById("cur_jindu").value = cur_jindu;
                alert('您今天已经新学了10个课时（450分钟），请明天再来学习新课程！为保证学习效果，每天最多新学10个课时。');
                top.location.href = 'mycourse.php';
            } else if (c == 'updating') {
                //videoPlayer.pause();
                document.getElementById("cur_jindu").value = cur_jindu;
                alert('本课程正在更新中，请明天再来学习该课程。课程更新不会影响您已有的学习进度。现在您可以先学习其他课程。');
                top.location.href = 'mycourse.php';
            } else if (c == 'is_pass') {
                //videoPlayer.pause();
                document.getElementById("cur_jindu").value = cur_jindu;
                alert('此用户当前无法登录。如有疑问，请联系您的班主任老师。');
                top.location.href = 'login.php';
            } else {
                document.getElementById("max_jindu").value = c;
                if (c < 120) {
                    document.getElementById('mj_m_01').innerHTML = parseInt(c) + ' 秒';
                    document.getElementById('mj_m_02').innerHTML = parseInt(c) + ' 秒';
                } else {
                    document.getElementById('mj_m_01').innerHTML = parseInt(c / 60) + ' 分钟';
                    document.getElementById('mj_m_02').innerHTML = parseInt(c / 60) + ' 分钟';
                }
                document.getElementById("cur_jindu").value = c;
                var shichang = document.getElementById("shichang").value;
                var is_kt = document.getElementById("is_kt").value;
                var isnot_st = document.getElementById("isnot_st").value;
                /* if (parseInt(cur_jindu) + 6 < parseInt(totalSeconds)) {
                    handle()
                } else {
                    document.getElementById("cur_jindu").value = parseInt(totalSeconds);
                    onplayend()
                }*/
            }
            return;
        });
    }


    function onplayend() {
        var jjwikiid = document.getElementById("jjwikiid").value;
        var TypeId = document.getElementById("TypeId").value;
        var cpidlist = document.getElementById("cpidlist").value;
        var cur_jindu = 999999;
        var userlog_id = document.getElementById("userlog_id").value;
        var sp_type = document.getElementById("sp_type").value;

        $.get("learn_jjsp.action.php", {
            TypeId: TypeId, jjwikiid: jjwikiid, cur_jindu: cur_jindu, userlog_id: userlog_id, r: Math.random()
        }, function (c) {
            if (c == 'video_img_yz') {
                player.pause();
                face_ewm(); return;
            } else if (c == 'video_yz') {
                CaptchaId_yz(); return;
            } else if (c == 'video_vtt') {
                $('.loggn').val('1');
                $('.loggns').val('1');
                player.pause();
                vtt(); return;
            } else if (c == '888888') {
                player.pause();
                alert('学习进度异常，请重新开始学习！！');
                window.location.reload();
            } else {
                document.getElementById("max_jindu").value = c;
                if (c < 120) {
                    document.getElementById('mj_m_01').innerHTML = parseInt(c) + ' 秒';
                    document.getElementById('mj_m_02').innerHTML = parseInt(c) + ' 秒';
                } else {
                    document.getElementById('mj_m_01').innerHTML = parseInt(c / 60) + ' 分钟';
                    document.getElementById('mj_m_02').innerHTML = parseInt(c / 60) + ' 分钟';
                }
                document.getElementById("cur_jindu").value = cur_jindu;
                //章节练习
                var is_zy = document.getElementById("is_zy").value;
                var isnot_st = document.getElementById("isnot_st").value;
                if (is_zy == 0 && isnot_st == 1) {
                    layer.open({
                        title: '提示信息',
                        content: '本章学习已完成，请学习下一章！',
                        offset: '10px'
                    });
                }
                if (is_zy == 0 && isnot_st == 0) {  //本讲无习题时，弹出提示
                    // alert ("本章学习已完成，请学习下一章！");
                    layer.open({
                        title: '提示信息',
                        content: '本章学习已完成，请学习下一章！',
                        offset: '10px'
                    });
                    return;
                }
            }
        });
    }
    // Your code here...
})();