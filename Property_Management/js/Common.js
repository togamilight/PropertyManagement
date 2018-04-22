var Common = {};

//公用popover属性
Common.popoverOptions = {
    container: "body",
    trigger: "manual"   //手动触发
};

//公用datepicker属性
Common.datepickerOptions = {
    autoclose: true,            //选择日期后自动关闭选择框
    startDate: "1970-01-01",    //最小日期
    endDate: "0d",              //最大日期为今天
    format: "yyyy-mm-dd",       //日期格式
    language: "zh-CN",          //语言：中文
    todayBtn: true,             //显示“今天”按钮
    todayHighlight: true,       //高亮今天
    container: "body"
}

// 设置datatables默认值
$.extend($.fn.dataTable.defaults, {
    autoWidth: false,
    scrollX: true,              //横向滚动
    ordering: false,            //排序
    lengthChange: false,        //选择每页条数
    searching: false,           //搜索
    serverSide: true,           //服务器端处理
    processing: true,           //显示处理中
    language: {                 //中文
        emptyTable: "无可用数据",
        infoEmpty: "没有数据可以显示",
        zeroRecords: "没有匹配的数据",
        infoFiltered: "(从总共 _MAX_ 条数据中过滤)",
        info: "_TOTAL_ 条数据中的第 _START_ 至 _END_ 条",
        processing: "数据正在加载中......",
        paginate: {
            next: "下一页",
            previous: "上一页"
        }
    }
});
//Common.datatablesOptions = {
//    scrollX: true,			//横向滚动
//    ordering: false,			//排序
//    lengthChange: false,		//选择每页条数
//    searching: false,			//搜索
//    serverSide: true,			//服务器端处理
//    processing: true,			//显示处理中
//};

//将后台传过来的日期字符串转换为Date格式
Common.parseDate = function (dateStr) {
    var str = dateStr.replace("/Date(", "").replace(")/", "");
    return new Date(parseInt(str));
}

//js的日期格式转换
Date.prototype.format = function (format) {
    var args = {
        "M+": this.getMonth() + 1,
        "d+": this.getDate(),
        "h+": this.getHours(),
        "m+": this.getMinutes(),
        "s+": this.getSeconds(),
        "q+": Math.floor((this.getMonth() + 3) / 3),  //quarter
        "S": this.getMilliseconds()
    };
    if (/(y+)/.test(format))
        format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var i in args) {
        var n = args[i];
        if (new RegExp("(" + i + ")").test(format))
            format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? n : ("00" + n).substr(("" + n).length));
    }
    return format;
};

//校验表单字段，传入表单的jquery对象
Common.validateForm = function ($form) {
    var $validateObj = $form.find("input[validate-type]");
    var isValid = true;

    $validateObj.each(function (index, element) {
        var $element = $(element);

        var type = $element.attr("validate-type");
        var types = type.split(" ");
        var value = $element.val();

        var msg = "";
        for (let i = 0; i < types.length && msg == ""; i++) {
            switch (types[i]) {
                case "required":
                    if (!value) {
                        msg = "该字段为必填";
                    }
                    break;

                case "length":
                    let minLen = $element.attr("validate-minLen");
                    let maxLen = $element.attr("validate-maxLen");

                    if ((minLen && !isNaN(minLen) && value.length < minLen) || (maxLen && !isNaN(maxLen) && value.length > maxLen)) {
                        let str = "";
                        if (minLen) {
                            str += "不能小于" + minLen + "位 ";
                        }
                        if (maxLen) {
                            str += "不能大于" + maxLen + "位";
                        }
                        msg = str;
                    }
                    break;

                case "uint":
                    if (!(/^[0-9]+$/.test(value))) {
                        msg = "该字段只能是正整数";
                    }
                    break;

                case "int":
                    if (!(/^\-?[0-9]+$/.test(value))) {
                        msg = "该字段只能是整数";
                    }
                    break;

                case "unumber":
                    if (!(/^[0-9]+\.?[0-9]*$/.test(value))) {
                        msg = "该字段只能是正数"
                    }
                    break;

                case "number":
                    if (!(/^\-?[0-9]+\.?[0-9]*$/.test(value))) {
                        msg = "该字段只能是数字"
                    }
                    break;

                case "range":
                    let min = $element.attr("validate-min");
                    let max = $element.attr("validate-max");

                    let num = Number(value);

                    if ((min && !isNaN(min) && num < min) || (max && !isNaN(max) && num > max)) {
                        let str = "";
                        if (min) {
                            str += "不能小于" + min + " ";
                        }
                        if (max) {
                            str += "不能大于" + max;
                        }
                        msg = str;
                    }
                    break;
                default:
                    break;
            }
        }

        if (msg) {
            $element.attr("data-content", msg);
            $element.popover("show");
            isValid = false;
        } else {
            $element.popover("hide");
        }
    });

    return isValid;
};

Common.ajaxError = function (XMLHttpRequest, textStatus, errorThrown) {
    // 状态码
    console.log("状态码：" + XMLHttpRequest.status + " 状态：" + XMLHttpRequest.readyState + " 错误信息：" + textStatus);
    
    alert("连接错误，请稍后重试");
};