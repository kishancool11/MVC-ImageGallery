$(function () {
    $('#mHomeSlide').parent().addClass("current");
    $('#mHomeSlide').parent().parent().parent().addClass("current");

    $("#btnUpdateSliderOrder").click(function () { 
        var MyList = $.map($(".orderLi"), function (item, index) {

            var tempList = new Object();
            tempList.Id = $(item).find(".iIdOrder").data('key');
            tempList.Order = $(".orderLi").length - index;
            return tempList;
        });
        var JsonList = JSON.stringify(MyList);
        $.ajax({
            type: "POST",
            contentType: "application/json",
            data: "{record:" + JsonList + "}",
            url: "/WebAdmin/Slider/Reorder",
            dataType: "json",
            success: function (data) {
                if (data)
                {
                    $('.test2').jGrowl('Image Re-ordered'); 
                } else {

                    $('.test2').jGrowl('Some Problem Here');
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(XMLHttpRequest + " , " + textStatus + " , " + errorThrown);
            }
        });
    });



    $(".iIdOrder").change(function () {
        var $checkbox = $(this);
        var nid = $checkbox.data("key");
         
        $.ajax({
            type: "POST",
            url: "/WebAdmin/Slider/ChangeActive",
            data: "{Id:" + nid + ",status: '" + $checkbox.is(":checked") + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                if ($checkbox.is(":checked")) {
                    $('.test2').jGrowl(msg + ' Maked as Active.');
                }
                else {
                    $('.test2').jGrowl(msg + ' Maked as InActive.');
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(textStatus + errorThrown);
            }
        });
    });

    var _bManager = new $.bootboxManager();
    $("#btnCreate").on("click", function (e) {
        var $this = $(this);
        e.preventDefault();
        _bManager.show({
            contentUrl: $this.attr("href"),
            title: "Create New",
            size: 'large',
            buttons: [{
                caption: 'Create',
                action: 'post',
                className: 'btn-success',
                callback: function (d) {
                    $.itrans.common.jGrowl(d.data.Caption + ' has been Saved');
                    $("#listId").load($("#listId").data("link"));
                    return true;
                }
            }]
        });
    });
});

 
