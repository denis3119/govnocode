
var pathname = window.location.pathname.split("/");
var t = pathname[pathname.length - 1];
if (Number(t))
    $(document).ready(LoadComments(pathname[pathname.length - 1]));

//TextView
var t3 = $("#TextView").markdown(
{
     autofocus: false, hiddenButtons: "", initialstate: "preview", fullscreen: false
}).initialstate = "preview";

if (pathname[pathname.length - 2].toLowerCase() === "details" &&
    pathname[pathname.length - 3].toLowerCase() === "publication")
{


    $(".md-editor").attr("class", "");
    var t = pathname[pathname.length - 1];
    if (Number(t)) {
        publicationRateGet(t);
        $(document).ready(LoadComments(t));
        $(".text-icon").hide();

    }
}

$(document).ready(function() {
    if (!$("#myCanvas").tagcanvas({
        textColour: "#000000",
        outlineThickness: 1,
        maxSpeed: 0.03,
        depth: 1
    })) {
        // TagCanvas failed to load
      $("#myCanvasContainer").hide();
    }
    // your other jQuery stuff here...
});

    var sampleTags;

function name(parameters) {
        var temp = [""];
        sampleTags = temp.concat(parameters);
        // alert(sampleTags);
        $("#myTags").tagit({
            availableTags: sampleTags,
            singleField: true,
            singleFieldNode: $("#TagString")
        });
        $("#myTags3").tagit({
            availableTags: sampleTags,
            singleField: true,
            singleFieldNode: $("#TagString"),
            readOnly: true,
            onTagClicked: function(event, ui) {
                // do something special
              //  alert();
                window.location = '/Tag/?tag=' + ui.tagLabel.replace("#", "%23");
            }
        });
    }
 $(document).ready(function() {
        $.ajax({
            type: "POST",
            url: "/Tag/All",
            data: {},
            beforeSend: function() {
                /* что-то сделать перед */
            },
            success: function(data) {
                /* обработать результат */
                // alert(data);
                name(data);
            },
            error: function() {
                /* обработать ошибку */
                //   alert("error send comment");
            }
        });


    });

function setUserName() {
        var userName = $("#userName").val();
        $.ajax({
            type: "POST",
            url: "/Profile/SetUserName",
            data: { name: userName },
            beforeSend: function() {
                /* что-то сделать перед */
            },
            success: function(data) {
                /* обработать результат */
                //  alert(data);
                if (data !== "False") {
                    $("#saveName").text("");
                    $("#saveName").show();

                    $("#saveName").text("Имя сохранено");
                    setTimeout(function() {

                        $("#saveName").hide();
                    }, 3000);
                } else {
                    $("#saveName").text("");
                    $("#saveName").show();

                    $("#saveName").text("Имя занято");
                    setTimeout(function() {

                        $("#saveName").hide();
                    }, 3000);

                }
            },
            error: function() {
                /* обработать ошибку */
                alert("error send comment");
            }
        });
    }

function hidebuttonrate(id) {
        $("a #img" + id).hide("true");
    }

function setlabelrank(id, data) {
        $("label #comm" + id).text(data);

        $("#text" + id).removeClass("alert-info");
        $("#text" + id).removeClass("alert-info-1");
        if (data < 0) {
            $("#text" + id).addClass("alert-info-1");
        } else {
            $("#text" + id).addClass("alert-info");
        }
    }

function up(idcomment) {
        $.ajax({
            type: "POST",
            url: "/Comment/Up",
            data: { idcomment: idcomment },
            beforeSend: function() {
                /* что-то сделать перед */
            },
            success: function(data) {
                /* обработать результат */
                //alert(data);
                setlabelrank(idcomment, data);
            },
            error: function() {
                /* обработать ошибку */
                alert("error send comment");
            }
        });
        hidebuttonrate(idcomment);
    }

function down(idcomment) {
        $.ajax({
            type: "POST",
            url: "/Comment/Down",
            data: { idcomment: idcomment },
            beforeSend: function() {
                /* что-то сделать перед */
            },
            success: function(data) {
                /* обработать результат */
                //  alert(data);
                setlabelrank(idcomment, data);

            },
            error: function() {
                /* обработать ошибку */
                alert("error send comment");
            }
        });
        hidebuttonrate(idcomment);
    }

function GetRate(idcomment) {
        $.ajax({
            type: "POST",
            url: "/Comment/Rate",
            data: { idcomment: idcomment },
            beforeSend: function() {
                /* что-то сделать перед */
            },
            success: function(data) {
                /* обработать результат */
                //  alert(data);
                return data;
            },
            error: function() {
                /* обработать ошибку */
                alert("error send comment");
            }
        });
    }

function addComment(id) {
        if ($("#Comment").val() !== "")
            $.ajax({
                type: "POST",
                url: "/Comment/Add",
                data: { text: $("#Comment").val(), id: id },
                beforeSend: function() {
                    /* что-то сделать перед */
                },
                success: function() {
                    /* обработать результат */
                    // data3 = data;
                    LoadComments(id);
                    $("#Comment").val("");
                },
                error: function() {
                    /* обработать ошибку */
                }
            });

        //Profile/details/91d97a9d-18b1-4357-89ba-bbcb3e2075fa
    };

    var data2;

function asd(data, idcomment) {

        if (data) {
            $("#edit" + idcomment).show();
        }
    }

function requestEdit(idcomment) {
        $.ajax({
            type: "POST",
            url: "/Comment/RequestEdit",
            data: { idcomment: idcomment },
            beforeSend: function() {
                /* что-то сделать перед */
            },
            success: function(data) {
                /* обработать результат */
                //alert(data);
                // setlabelrank(idcomment, data);
                //  alert(data);
                //data2 = data;
                //return data;
                asd(data, idcomment);
            },
            error: function() {
                /* обработать ошибку */
                // alert("error send comment");
            }
        });
        //return data2;

    }

function LoadComments(id) {
        ;
        $.ajax({
            type: "POST",
            url: "/Comment/All",
            data: { id: id },
            beforeSend: function() {
                /* что-то сделать перед */
            },
            success: function(data) {
                /* обработать результат */
                // data3 = data;

                viewcomment(data);
                //  viewcomment(data);
            },
            error: function() {
                /* обработать ошибку */
            }
        });

        //Profile/details/91d97a9d-18b1-4357-89ba-bbcb3e2075fa
    };


function editComment(idcomment) {
        $("#myModal").modal({
            keyboard: false
        });
        $("#commentEditModal").val($("#text" + idcomment).val());
        idCommentEdit = idcomment;
    }

function deleteComment() {

        $.ajax({
            type: "POST",
            url: "/Comment/Delete",
            data: { id: idCommentEdit },
            beforeSend: function() {
                /* что-то сделать перед */
            },
            success: function(data) {
                /* обработать результат */
                // data3 = data
                if (data) {
                    //  alert("update true");
                    $("#modalClose").click();
                    //  $("#text" + idcomment).val(text);
                    $("#text" + idCommentEdit).val("deleted");
                    $("#text" + idCommentEdit).removeClass("alert-info").removeClass("alert-info-1").addClass("alert-danger").css("width", "100%");
                    $("#text" + idCommentEdit).closest("div").css("width", "100%");
                    $("#" + idCommentEdit).hide();
                    $("#edit" + idCommentEdit).hide();
                }

                //  viewcomment(data);
            },
            error: function() {
                /* обработать ошибку */
            }
        });
    }

function mousOver0Star() {
        // alert("6");

        $("#star1").attr("src", "../../Images/starOff.png");
        $("#star2").attr("src", "../../Images/starOff.png");
        $("#star3").attr("src", "../../Images/starOff.png");
        $("#star4").attr("src", "../../Images/starOff.png");
        $("#star5").attr("src", "../../Images/starOff.png");

    }

function mousOver1Star() {
        // alert("6");
        mousOver0Star();
        $("#star1").attr("src", "../../Images/starOn.png");

    }

function mousOver2Star() {
        mousOver1Star();
        $("#star2").attr("src", "../../Images/starOn.png");
    }

function mousOver3Star() {
        mousOver2Star();
        $("#star3").attr("src", "../../Images/starOn.png");
    }

function mousOver4Star() {
        mousOver3Star();
        $("#star4").attr("src", "../../Images/starOn.png");
    }

function mousOver5Star() {
        mousOver4Star();
        $("#star5").attr("src", "../../Images/starOn.png");
    }

function publicationRateSet(rate, idPublication) {

        $.ajax({
            type: "POST",
            url: "/Publication/UpRate",
            data: { rate: rate, idPublication: idPublication },
            beforeSend: function() {
                /* что-то сделать перед */
            },
            success: function(data) {
                /* обработать результат */
                // data3 = data
                if (data) {
                    $("#star1").attr("onmouseover", "").attr("onmouseclick", "");
                    $("#star2").attr("onmouseover", "").attr("onmouseclick", "");
                    $("#star3").attr("onmouseover", "").attr("onmouseclick", "");
                    $("#star4").attr("onmouseover", "").attr("onmouseclick", "");
                    $("#star5").attr("onmouseover", "").attr("onmouseclick", "");
                    publicationRateGet(idPublication);
                }

                //  viewcomment(data);
            },
            error: function() {
                /* обработать ошибку */
            }
        });
    }

function publicationRateGet(id) {

        $.ajax({
            type: "POST",
            url: "/Publication/GetRate",
            data: { idPublication: id },
            beforeSend: function() {
                /* что-то сделать перед */
            },
            success: function(data) {
                /* обработать результат */
                // data3 = data
                // alert(data);
                switch (data) {
                case "0":
                    mousOver0Star();
                    break;
                case "1":
                    mousOver1Star();
                    break;
                case "2":
                    mousOver2Star();
                    break;
                case "3":
                    mousOver3Star();
                    break;
                case "4":
                    mousOver4Star();
                    break;
                case "5":
                    mousOver5Star();
                    break;
                default:

                }
                //  viewcomment(data);
            },
            error: function() {
                /* обработать ошибку */
            }
        });
    }

function updateComment() {
        var text = $("#commentEditModal").val();
        var idcomment = idCommentEdit;
        $.ajax({
            type: "POST",
            url: "/Comment/Update",
            data: { text: text, id: idcomment },
            beforeSend: function() {
                /* что-то сделать перед */
            },
            success: function(data) {
                /* обработать результат */
                // data3 = data
                if (data) {
                    //  alert("update true");
                    $("#modalClose").click();
                    $("#text" + idcomment).val(text);
                }

                //  viewcomment(data);
            },
            error: function() {
                /* обработать ошибку */
            }
        });
    }

var idCommentEdit;
function addZero(i) {
        if (i < 10) {
            i = "0" + i;
        }
        return i;
}

function setImageLink() {
    var link = $("#linkImage").val();
    $.ajax({
        type: "POST",
        url: "/Profile/SetLinkImage",
        data: { image: link},
        beforeSend: function() {
            /* что-то сделать перед */
        },
        success: function(data) {
            /* обработать результат */
            // data3 = data
            if (data) {

            }

            //  viewcomment(data);
        },
        error: function() {
            /* обработать ошибку */
        }
    });
}
function viewcomment(data) {

        var res = "";
        for (var index in data) {

            if (data.hasOwnProperty(index)) {

                var attr = data[index];
                var id = attr["Id"];
                var upfunc = "up(" + id + ");";
                
                var idedit = "edit" + id;
                requestEdit(id);
                var downfunc = "down(" + attr["Id"] + ");";
                var idcomment = "comm" + attr["Id"];
                var idtext = "text" + attr["Id"];
                var idimg = "img" + attr["Id"];
                var x2 = attr["Date"];
                var x = [{ "id": 1, "start": x2 }];

                var date = new Date(x[0].start.match(/\d+/)[0] * 1);
                
                var rateint = attr["Rate"];
                // var idcommentint = attr["Id"];
                var negative = "";
                if (rateint < 0) {
                    negative = "-1";
                }

                var edit = "<tr ><td><a><img  style=\"display:none\" id=\"" + idedit + "\" onclick=\"editComment('" + id + "');\" src=\"../../Images/edit.png\" alt=\"\"/></a></td></tr>";

                var dateAndTime = "<div style=\"margin-left:86%;\"><label class=\"text text-info\">" + addZero(date.getHours()).toString() + "." + addZero(date.getMinutes("")).toString() + "</label> <label class=\"text text-muted\">" + date.getDate().toString() + "." + addZero(date.getUTCMonth() + 1).toString()
                    + "." + date.getFullYear().toString() + "</label>"+edit+"</div>";
                var voted = "";
                var nameurl = "";
                if (attr["Voted"]) {
                    voted = "hidden=\"true\"";
                    //  alert("true");
                    upfunc = "";
                } else {
                    voted = "";
                    upfunc = "up(" + attr["Id"] + ");";
                }

                var rate = "" +
                    "<table >" +
                    "<tr><a><img id=" + idimg + " " + voted + " onclick=" + upfunc + " src=\"../../Images/plus.png\" alt=\"\"/></a></tr>" +
                    "<tr><label id=\"" + idcomment + "\" class=\"label label-info\">" + rateint + "</label></tr>" +
                    "<tr style=\"\"><a><img id=\"" + idimg + "\" " + voted + " onclick=" + downfunc + " src=\"../../Images/minus.png\" alt=\"\" /></a></tr>" +
                    
                    "</table>";

                nameurl += "<a style=\"margin-left:0%;\"  href=\"/Profile/details/" + attr["IdUser"] + "\"><img src=\"http://www.x-bikers.ru/forum/avatars/309593.gif\" alt=\"admin\" width=\"32\" height=\"32\" class=\"img-rounded\">" + attr["UserName"] + "</a>";
                var row = rows(attr["Text"]);
                res += "<div class=\"well-sm well-1\"  style=\"border: 1px;padding: 1px\">" +
                    "<div class=\"container row\">" +
                    "<div style=\"margin-top:0;\" id='" + id + "' class=\"col-lg-1 text-center\">" +
                    "" + nameurl + "" +
                    "<div><label class=\"text-info\">" + rate + "</label></div>" +
                    "</div>" +
                    "<div class=\"col-lg-11\" style=\"max-width: 100%\">" + dateAndTime +
                    "<textarea id=\"" + idtext + "\" rows=\"" + row + "\" style=\"width: 100%; max-width: 100%\" readonly class=\" alert-info" + negative + " alert\">"
                    + attr["Text"] + "</textarea>"+"</div>" +
                    "</div>" +
                    "</div>";
            }
        }
        $("#view").html(res);


    }

function ChahgeImage()
{
    var image = $("#linkImage").val();
    $("#image").attr("src", image);
}
function rows(str) {
        var getText = str;
        var getRegs = getText.match(/^.*(\r\n|\n|$)/gim);
        var setText = "";
        for (var i = 0; i < getRegs.length; i++) {
            getText = getRegs[i].replace(/\r|\n/g, "");
            setText += getText.length ? Math.ceil(getText.length / 187) : 1;
        }

        if (setText > 10) setText = 4;
        return setText;
    };



//$("#TextView").attr("data-provide", "'markdown-preview");
