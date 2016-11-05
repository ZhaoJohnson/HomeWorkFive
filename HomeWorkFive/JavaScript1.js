// 显示到页面
display: function (x) {
    x = (x != true) ? false : true;
    var _t = this,
    _d = _t.cache.data,
    _c = _t.options.cells,
    __t = _t.options.cate,
    _p = _t.cache.page || 1,
    _ps = _t.options.pagesize || 50,
    _soft = _t.options.isSoft || false,
    _body = _t.cache.tbody,
    _head = _t.cache.thead;
    // clear data
    var trs = _body.getElementsByTagName("tr");
    for (var i = trs.length - 1; i >= 0; i--) {
        _body.removeChild(trs[i]);
    }

    var rowTp = _body.insertRow(-1);
    for (var i = 0; i < _c; i++) {
        var cell = rowTp.insertCell(i);
    }

    var _totle = _t.cache.count;

    document.getElementById("list_num").innerHTML = _t.cache.count;
    // bind data
    if (_totle == 0) {
        _t.showNoData();
    }

    // 每年的5月4日之前为true
    var nextYear = false;
    if (_totle > 0) {
        //等待修改
        for (var i = 0; i < _d.length; i++) {
            var data = _d[i];

            var row = rowTp.cloneNode(true);
            _body.appendChild(row);
            var _class = (i % 2 == 0) ? "" : "odd";

            var _arr = ["上调", "下调", "首次", "维持", "无"],
            _code = data.secuFullCode,
            _name = data.secuName,
            _urldate = data.datetime,
            _date = data.datetime,
            _grade = data.rate,
            _change = data.change,
            _jg = data.insName,
            _jgid = data.insCode;
            var year = data.profitYear;
            if (year != null && year != "")
            {
                document.getElementById("YearOne").innerHTML = parseInt(year) + 1+"预测";
                document.getElementById("YearTwo").innerHTML = parseInt(year) + 2 + "预测";
            }
            _code = _code.split('.')[0];
            var shouyi = data.sys;
            if (shouyi != null && shouyi.length > 1) {
                //收益1
                _ycxj1 = (shouyi[0] == "" || isNaN(shouyi[0])) ? "-" : parseFloat(shouyi[0]).toFixed(3);
                if (_ycxj1 == "NaN")
                {
                    _ycxj1 = "-";
                }
                //收益2
                _ycxj2 = (shouyi[1] == "" || isNaN(shouyi[1])) ? "-" : parseFloat(shouyi[1]).toFixed(3);
                if (_ycxj2 == "NaN") {
                    _ycxj2 = "-";
                }
            }

            var shiyinglv = data.syls;
            if (shiyinglv != null && shiyinglv.length > 1) {
                //市盈率1
                _syl1 = (shiyinglv[0] == "" || isNaN(shiyinglv[0]) || parseFloat(shiyinglv[0]) <= 0) ? "-" : parseFloat(shiyinglv[0]).toFixed(2);
                if (_syl1 == "NaN") {
                    _syl1 = "-";
                }
                //市盈率2
                _syl2 = (shiyinglv[1] == "" || isNaN(shiyinglv[1]) || parseFloat(shiyinglv[1]) <= 0) ? "-" : parseFloat(shiyinglv[1]).toFixed(2);
                if (_syl2 == "NaN") {
                    _syl2 = "-";
                }
            }

            var price = data.newPrice;

            if (_syl1 == "-" && _ycxj1 != "-")
            {
                _syl1 = parseFloat(_ycxj1 * price).toFixed(2);
                if (_syl1 == "NaN" || parseFloat(_syl1) <=0) {
                    _syl1 = "-";
                }
            }

            if (_syl2 == "-" && _ycxj2 != "-") {
                _syl2 = parseFloat(_ycxj2 * price).toFixed(2);
                if (_syl2 == "NaN" || parseFloat(_syl2) <= 0) {
                    _syl2 = "-";
                }
            }

            _id = data.infoCode,
            _title = data.title,
            _tit = data.title;
            _date = _date.split("T")[0];
            _urldate = _date.split("T")[0];
            _date = (/\d{4}-\d{1,2}-\d{1,2}/i).test(_date) ? "<span title=\"" + _date + "\" class=\"txt\">"
            + new Date(Date.parse(_date.replace(/-/ig, "/"))).format("MM-dd") + "</span>" : "-";

            _urldate = (/\d{4}-\d{1,2}-\d{1,2}/i).test(_urldate)
            ? new Date(Date.parse(_urldate.replace(/-/ig, "/"))).format("yyyyMMdd") : "detail";

            var linkPath = "/report/";
            if (_soft)
                linkPath = "/soft/reportNew/";

            var _qs_link = linkPath + _jg + ".html",
                _gg_link = linkPath + _code + ".html",
                _xx_link = linkPath + _urldate + "/" + _id + ".html",
                _jg_link = linkPath + _jgid + "_0.html";

            row.className = _class;
            row.onmouseover = function () {
                this.className = "over";
            }

            row.onmouseout = function (o, _c) {
                o.className = _c;
            }.bind(this, row, _class)

            row.cells[0].innerHTML = ((_p - 1) * _ps + i + 1);
            row.cells[1].innerHTML = _date;

            if (!_soft) {
                var _hq_link = "http://quote.eastmoney.com/" + _code + ".html",
                    _gb_link = "http://guba.eastmoney.com/list," + _code + ".html";
                row.cells[2].innerHTML = "<a href=\"" + _hq_link + "\">" + _code + "</a>";//代码
                row.cells[3].innerHTML = "<a href=\"" + _hq_link + "\">" + _name + "</a>";//名称
                row.cells[4].innerHTML = "<a href=\"" + _gg_link + "\" class=\"red\">详细</a> <a href=\"" + _gb_link + "\">股吧</a> ";//相关
                row.cells[5].innerHTML = "<div class=\"report_tit\"><a href=\"" + _xx_link + "\" title=\"" + _title + "\">" + _tit + "</a></div>";//研报
            }
            else {
                var _hq_link_prop = ' hidefocus="true" href="javascript:;" onclick="external.OnClickEvent(this.rel);" rel="return(false &amp;&amp; [].push(\'#type=01&amp;&amp;code=' + tiny.GetMktByCode(_code) + _code + '\'))" ';
                row.cells[2].innerHTML = "<a " + _hq_link_prop + ">" + _code + "</a>";//代码
                row.cells[3].innerHTML = "<a " + _hq_link_prop + ">" + _name + "</a>";//名称
                row.cells[4].style.display = "none";
                row.cells[5].innerHTML = '<div class="report_tit"><a hidefocus="true" href="javascript:;" onclick="external.OnClickEvent(this.rel);" rel="return(false &amp;&amp; [].push(\'#type=03&amp;title=' + _name + '研报正文&amp;size=1024x650&amp;url=' + location.origin + _xx_link + '\'))" title="' + _title + '">' + _tit + '</a></div>';//研报
            }

            row.cells[6].innerHTML = "" + _grade == "" ? "-" : _grade + "";
            row.cells[7].innerHTML = "" + _change == "" ? "无" : _change + "";
            row.cells[8].innerHTML = "<a href=\"" + _jg_link + "\">" + _jg + "</a>";

            row.cells[9].innerHTML = "" + _ycxj1 + "";
            row.cells[10].innerHTML = "" + _syl1 + "";

            row.cells[11].innerHTML = "" + _ycxj2 + "";
            row.cells[12].innerHTML = "" + _syl2 + "";
        }
    }
    _body.removeChild(rowTp);
    _t.pageit();
}