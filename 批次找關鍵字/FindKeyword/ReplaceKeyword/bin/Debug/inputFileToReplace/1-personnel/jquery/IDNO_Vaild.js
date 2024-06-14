//--start--此區塊可單獨放在 JS 檔，再 Includ 引用---
function idChech(id) {

    if (checkid(id) == false) {
        errorMsg = "您的身分證字號位數不對!!";
        return false;
    }

    if (chfastid(id) == false) {
        errorMsg = "您的身分證字號第一碼必須是大寫的英文字母!!";
        return false;
    }

    if (ch12fastid(id) == false) {
        errorMsg = "您的身分證字號第二碼有問題 !";
        return false;
    }

    if (firstlettererr(id) == false) {
        errorMsg = "您的身分證字號第一碼必須是大寫的英文字母!";
        return false;
    }

    if (idmanber(id) == false) {
        errorMsg = "您的身分證字號後9碼應為數字!!";
        return false;
    }

    if (idchackok(id) == false) {
        errorMsg = "您的身分證字號有問題 !";
        return false;
    }
    return true;
}

function firstlettererr(id) {
    var fl = id.value.substr(0, 1);
    var T = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; //*24個*// 
    var smp = id.value.substr(0, 1)
    if (T.indexOf(smp) == -1) { return false; } else { return true; }
}

function checkid(id) {
    if (id.value.length == 10)
    { return true; } else { return false; }
}

function chfastid(id) {
    var c = id.value.charAt(0);
    if (c < "A" || c > "Z")
    { return false; } else { return true; }
}

function ch12fastid(id) {
    var c = id.value.charAt(1);
    if (c != "1" && c != "2")
    { return false; } else { return true; }
}

function idmanber(id) { //=====後九碼為數字// 
    var bmp;
    var d = "0123456789";
    var bab = id.value.length - 1;
    for (var i = 1; i <= bab; i++) {
        bmp = id.value.substr(i, 1)
        if (d.indexOf(bmp) == -1) { return false; }
    }
    if (id.value != "0")
    { return true; } else
    { return false; }
}

function idchackok(id) { //規則// 
    var alph = new Array("A", "B", "C", "D", "E", "F", "G", "H", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "U",
"V", "X", "Y", "W", "Z", "I", "O");
    var num = new Array("10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25",
"26", "27", "28", "29", "30", "31", "32", "33", "34", "35");
    var n = 0;
    for (i = 0; i < alph.length; i++)
        if (id.value.charAt(0) == alph[i])
        n = i;
    var tot1 = parseFloat(num[n].charAt(0)) + (parseFloat(num[n].charAt(1)) * 9);
    var tot2 = 0;
    for (i = 1; i < id.value.length - 1; i++)
        tot2 = tot2 + parseFloat(id.value.charAt(i)) * (9 - i);
    var tot3 = parseFloat(id.value.charAt(9));
    var tot4 = tot1 + tot2 + tot3;
    if ((tot4 % 10) != 0)
    { return false; } else { return true; }
}
//-------end--------