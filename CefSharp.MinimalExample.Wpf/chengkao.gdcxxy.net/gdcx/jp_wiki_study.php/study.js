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
                alert('ϵͳ��ʱ���û����������ط���¼�������µ�¼��ע�⣺�û�ͬʱ�򿪶��ѧϰҳ��Ҳ������ϵͳ�Զ��˳���');
                top.location.href = 'login.php';
            } else if (c == '888888') {
                //videoPlayer.pause();
                document.getElementById("cur_jindu").value = cur_jindu;
                alert('ѧϰ�����쳣�������¿�ʼѧϰ��');
                window.location.reload();
            } else if (c == 'same') {
                //videoPlayer.pause();
                document.getElementById("cur_jindu").value = cur_jindu;
                alert('Ϊ��֤ѧϰЧ����������ͬʱѧϰ����γ̣�ϵͳ���Զ��˳��������µ�¼��');
                top.location.href = 'login.php';
            } else if (c == 'over10') {
                //videoPlayer.pause();
                document.getElementById("cur_jindu").value = cur_jindu;
                alert('�������Ѿ���ѧ��10����ʱ��450���ӣ�������������ѧϰ�¿γ̣�Ϊ��֤ѧϰЧ����ÿ�������ѧ10����ʱ��');
                top.location.href = 'mycourse.php';
            } else if (c == 'updating') {
                //videoPlayer.pause();
                document.getElementById("cur_jindu").value = cur_jindu;
                alert('���γ����ڸ����У�����������ѧϰ�ÿγ̡��γ̸��²���Ӱ�������е�ѧϰ���ȡ�������������ѧϰ�����γ̡�');
                top.location.href = 'mycourse.php';
            } else if (c == 'is_pass') {
                //videoPlayer.pause();
                document.getElementById("cur_jindu").value = cur_jindu;
                alert('���û���ǰ�޷���¼���������ʣ�����ϵ���İ�������ʦ��');
                top.location.href = 'login.php';
            } else {
                document.getElementById("max_jindu").value = c;
                if (c < 120) {
                    document.getElementById('mj_m_01').innerHTML = parseInt(c) + ' ��';
                    document.getElementById('mj_m_02').innerHTML = parseInt(c) + ' ��';
                } else {
                    document.getElementById('mj_m_01').innerHTML = parseInt(c / 60) + ' ����';
                    document.getElementById('mj_m_02').innerHTML = parseInt(c / 60) + ' ����';
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
                alert('ѧϰ�����쳣�������¿�ʼѧϰ����');
                window.location.reload();
            } else {
                document.getElementById("max_jindu").value = c;
                if (c < 120) {
                    document.getElementById('mj_m_01').innerHTML = parseInt(c) + ' ��';
                    document.getElementById('mj_m_02').innerHTML = parseInt(c) + ' ��';
                } else {
                    document.getElementById('mj_m_01').innerHTML = parseInt(c / 60) + ' ����';
                    document.getElementById('mj_m_02').innerHTML = parseInt(c / 60) + ' ����';
                }
                document.getElementById("cur_jindu").value = cur_jindu;
                //�½���ϰ
                var is_zy = document.getElementById("is_zy").value;
                var isnot_st = document.getElementById("isnot_st").value;
                if (is_zy == 0 && isnot_st == 1) {
                    layer.open({
                        title: '��ʾ��Ϣ',
                        content: '����ѧϰ����ɣ���ѧϰ��һ�£�',
                        offset: '10px'
                    });
                }
                if (is_zy == 0 && isnot_st == 0) {  //������ϰ��ʱ��������ʾ
                    // alert ("����ѧϰ����ɣ���ѧϰ��һ�£�");
                    layer.open({
                        title: '��ʾ��Ϣ',
                        content: '����ѧϰ����ɣ���ѧϰ��һ�£�',
                        offset: '10px'
                    });
                    return;
                }
            }
        });
    }
    // Your code here...
})();