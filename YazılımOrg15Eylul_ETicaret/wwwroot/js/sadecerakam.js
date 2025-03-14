
//sadece rakam girilmesi gereken alanı kontrol edecez

function numberonly(myfield, e, dec) {
    var key;             //basılan tuşun ascii kodunu değişkene alıyoruz
    var keychar;         //basılan tuşun ascii kodunu karaktere çevirip bu değişkene atıyoruz


    if (window.event)
        key = window.event.keyCode;    //ascii kodu aldık
    else if (e)
        key = e.wich;
    else
        return true;

    keychar = String.fromCharCode(key);   //karaktere çevirdik

    //control keys bazı tuşlara izin veriyoruz 
    /*
    *0 null
    *8 backspace (silme)
    *9 tab
    *13 enter
    * 27 escape     ESC
    
    
    */

    if ((key == null) || (key == 0) || (key == 8) || (key == 9) || (key == 13) || (key == 27))
        return true;


    //numbers 

    else if (("0123456789").lastIndexOf(keychar) > -1)
        return true;

    else if (dec && (keychar ==".")  )
    {
        myfield.form.elements[dec].focus();
        alert("Lütfen sadece rakam giriniz!");
        return false;
    }
    else
    {
        alert("Lütfen sadece rakam giriniz!");
        return false;
    }

}