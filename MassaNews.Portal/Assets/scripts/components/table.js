/*
tbl.js
Desenvolvido por Tabela Fácil
www.tabelafacil.com

As funções e variáveis desse script começam com o prefixo "tbl", evite funções com o mesmo prefixo no seu código.
*/

function tblOverCol(table,col,rows){
  var i=0;
  for(i=0;i<=rows;i++){
    cell=document.getElementById(table+i+"x"+col);tblAddClass(cell,"tbl-hover");
  }
}

function tblOutCol(table,col,rows){
  var i=0;
  for(i=0;i<=rows;i++){
    cell=document.getElementById(table+i+"x"+col);tblRemoveClass(cell,"tbl-hover");
  }
}

function tblHasClass(ele,cls){
  return ele.className.match(new RegExp('(\\s|^)'+cls+'(\\s|$)'));
}

function tblAddClass(ele,cls){
  if(!this.tblHasClass(ele,cls))ele.className+=" "+cls;
}

function tblRemoveClass(ele,cls){
  if(tblHasClass(ele,cls)){
    var reg=new RegExp('(\\s|^)'+cls+'(\\s|$)');
    ele.className=ele.className.replace(reg,' ');
  }
}

var stIsIE = /*@cc_on!@*/false;

tblSortTable = {
  init: function() {
    if (arguments.callee.done) return;
    arguments.callee.done = true;
    if (_timer) clearInterval(_timer);
    
    if (!document.createElement || !document.getElementsByTagName) return;
    
    tblSortTable.DATE_RE = /^(\d\d?)[\/\.-](\d\d?)[\/\.-]((\d\d)?\d\d)$/;
    
    forEach(document.getElementsByTagName('table'), function(table) {
      if (table.className.search(/\btbl-sortable\b/) != -1) {
        tblSortTable.makeSortable(table);
      }
    });
    
  },
  
  makeSortable: function(table) {
    if (table.getElementsByTagName('thead').length === 0) {
      the = document.createElement('thead');
      the.appendChild(table.rows[0]);
      table.insertBefore(the,table.firstChild);
    }
    if (table.tHead === null) table.tHead = table.getElementsByTagName('thead')[0];
    
    if (table.tHead.rows.length != 1) return;    
    sortbottomrows = [];
    for (var i=0; i<table.rows.length; i++) {
      if (table.rows[i].className.search(/\bsortbottom\b/) != -1) {
        sortbottomrows[sortbottomrows.length] = table.rows[i];
      }
    }
    if (sortbottomrows) {
      if (table.tFoot === null) {
        tfo = document.createElement('tfoot');
        table.appendChild(tfo);
      }
      for (var i=0; i<sortbottomrows.length; i++) {
        tfo.appendChild(sortbottomrows[i]);
      }
      delete sortbottomrows;
    }
    
    headrow = table.tHead.rows[0].cells;
    for (var i=0; i<headrow.length; i++) {
      if (!headrow[i].className.match(/\btblSortTable_nosort\b/)) {        mtch = headrow[i].className.match(/\btblSortTable_([a-z0-9]+)\b/);
        if (mtch) { override = mtch[1]; }
        if (mtch && typeof tblSortTable["sort_"+override] == 'function') {
          headrow[i].tblSortTable_sortfunction = tblSortTable["sort_"+override];
        } else {
          headrow[i].tblSortTable_sortfunction = tblSortTable.guessType(table,i);
        }
        headrow[i].tblSortTable_columnindex = i;
        headrow[i].tblSortTable_tbody = table.tBodies[0];
        dean_addEvent(headrow[i],"click", function(e) {

          if (this.className.search(/\btbl-sort-asce\b/) != -1) {
            tblSortTable.reverse(this.tblSortTable_tbody);
            this.className = this.className.replace('tbl-sort-asce', 'tbl-sort-desce');
            return;
          }
          if (this.className.search(/\btbl-sort-desce\b/) != -1) {
            tblSortTable.reverse(this.tblSortTable_tbody);
            this.className = this.className.replace('tbl-sort-desce', 'tbl-sort-asce');
            return;
          }
          
          theadrow = this.parentNode;
          forEach(theadrow.childNodes, function(cell) {
            if (cell.nodeType == 1) {
              cell.className = cell.className.replace('tbl-sort-desce','');
              cell.className = cell.className.replace('tbl-sort-asce','');
            }
          });
          this.className += ' tbl-sort-asce';

          row_array = [];
          col = this.tblSortTable_columnindex;
          rows = this.tblSortTable_tbody.rows;
          for (var j=0; j<rows.length; j++) {
            row_array[row_array.length] = [tblSortTable.getInnerText(rows[j].cells[col]), rows[j]];
          }
          row_array.sort(this.tblSortTable_sortfunction);
          tb = this.tblSortTable_tbody;
          for (var j=0; j<row_array.length; j++) {
            tb.appendChild(row_array[j][1]);
          }
          
          delete row_array;
        });
      }
    }
  },
  
  guessType: function(table, column) {
    sortfn = tblSortTable.sort_alpha;
    for (var i=0; i<table.tBodies[0].rows.length; i++) {
      text = tblSortTable.getInnerText(table.tBodies[0].rows[i].cells[column]);
      if (text !== '') {
        if (text.match(/^-?[£$¤]?[\d,.]+%?$/)) {
          return tblSortTable.sort_numeric;
        }
        possdate = text.match(tblSortTable.DATE_RE);
        if (possdate) {
          first = parseInt(possdate[1]);
          second = parseInt(possdate[2]);
          if (first > 12) {
            return tblSortTable.sort_ddmm;
          } else if (second > 12) {
            return tblSortTable.sort_mmdd;
          } else {
            sortfn = tblSortTable.sort_ddmm;
          }
        }
      }
    }
    return sortfn;
  },
  
  getInnerText: function(node) {
    
    hasInputs = (typeof node.getElementsByTagName == 'function') && node.getElementsByTagName('input').length;
    
    if (node.getAttribute("tblSortTable_customkey") !== null) {
      return node.getAttribute("tblSortTable_customkey");
    }
    else if (typeof node.textContent != 'undefined' && !hasInputs) {
      return node.textContent.replace(/^\s+|\s+$/g, '');
    }
    else if (typeof node.innerText != 'undefined' && !hasInputs) {
      return node.innerText.replace(/^\s+|\s+$/g, '');
    }
    else if (typeof node.text != 'undefined' && !hasInputs) {
      return node.text.replace(/^\s+|\s+$/g, '');
    }
    else {
      switch (node.nodeType) {
        case 3:
          if (node.nodeName.toLowerCase() == 'input') {
            return node.value.replace(/^\s+|\s+$/g, '');
          }
        case 4:
          return node.nodeValue.replace(/^\s+|\s+$/g, '');
          break;
        case 1:
        case 11:
          var innerText = '';
          for (var i = 0; i < node.childNodes.length; i++) {
            innerText += tblSortTable.getInnerText(node.childNodes[i]);
          }
          return innerText.replace(/^\s+|\s+$/g, '');
          break;
        default:
          return '';
      }
    }
  },
  
  reverse: function(tbody) {
    newrows = [];
    for (var i=0; i<tbody.rows.length; i++) {
      newrows[newrows.length] = tbody.rows[i];
    }
    for (var i=newrows.length-1; i>=0; i--) {
       tbody.appendChild(newrows[i]);
    }
    delete newrows;
  },
  
  sort_numeric: function(a,b) {
    aa = parseFloat(a[0].replace(/[^0-9.-]/g,''));
    if (isNaN(aa)) aa = 0;
    bb = parseFloat(b[0].replace(/[^0-9.-]/g,'')); 
    if (isNaN(bb)) bb = 0;
    return aa-bb;
  },
  sort_alpha: function(a,b) {
    if (a[0]==b[0]) return 0;
    if (a[0]<b[0]) return -1;
    return 1;
  },
  sort_ddmm: function(a,b) {
    mtch = a[0].match(tblSortTable.DATE_RE);
    y = mtch[3]; m = mtch[2]; d = mtch[1];
    if (m.length == 1) m = '0'+m;
    if (d.length == 1) d = '0'+d;
    dt1 = y+m+d;
    mtch = b[0].match(tblSortTable.DATE_RE);
    y = mtch[3]; m = mtch[2]; d = mtch[1];
    if (m.length == 1) m = '0'+m;
    if (d.length == 1) d = '0'+d;
    dt2 = y+m+d;
    if (dt1==dt2) return 0;
    if (dt1<dt2) return -1;
    return 1;
  },
  sort_mmdd: function(a,b) {
    mtch = a[0].match(tblSortTable.DATE_RE);
    y = mtch[3]; d = mtch[2]; m = mtch[1];
    if (m.length == 1) m = '0'+m;
    if (d.length == 1) d = '0'+d;
    dt1 = y+m+d;
    mtch = b[0].match(tblSortTable.DATE_RE);
    y = mtch[3]; d = mtch[2]; m = mtch[1];
    if (m.length == 1) m = '0'+m;
    if (d.length == 1) d = '0'+d;
    dt2 = y+m+d;
    if (dt1==dt2) return 0;
    if (dt1<dt2) return -1;
    return 1;
  },
  
  shaker_sort: function(list, comp_func) {
    var b = 0;
    var t = list.length - 1;
    var swap = true;

    while(swap) {
      swap = false;
      for(var i = b; i < t; ++i) {
        if ( comp_func(list[i], list[i+1]) > 0 ) {
          var q = list[i]; list[i] = list[i+1]; list[i+1] = q;
          swap = true;
        }
      }
      t--;

      if (!swap) break;

      for(var i = t; i > b; --i) {
        if ( comp_func(list[i], list[i-1]) < 0 ) {
          var q = list[i]; list[i] = list[i-1]; list[i-1] = q;
          swap = true;
        }
      }
      b++;
    }
  }  
};

if (document.addEventListener) {
  document.addEventListener("DOMContentLoaded", tblSortTable.init, false);
}

/*@cc_on @*/
/*@if (@_win32)
    document.write("<script id=__ie_onload defer src=javascript:void(0)><\/script>");
    var script = document.getElementById("__ie_onload");
    script.onreadystatechange = function() {
        if (this.readyState == "complete") {
            tblSortTable.init();        }
    };
/*@end @*/

if (/WebKit/i.test(navigator.userAgent)) {
  var _timer = setInterval(function() {
    if (/loaded|complete/.test(document.readyState)) {
      tblSortTable.init();
    }
  }, 10);
}

window.onload = tblSortTable.init;

function dean_addEvent(element, type, handler) {
  if (element.addEventListener) {
    element.addEventListener(type, handler, false);
  } else {
    if (!handler.$$guid) handler.$$guid = dean_addEvent.guid++;
    if (!element.events) element.events = {};
    var handlers = element.events[type];
    if (!handlers) {
      handlers = element.events[type] = {};
      if (element["on" + type]) {
        handlers[0] = element["on" + type];
      }
    }
    handlers[handler.$$guid] = handler;
    element["on" + type] = handleEvent;
  }
}
dean_addEvent.guid = 1;

function removeEvent(element, type, handler) {
  if (element.removeEventListener) {
    element.removeEventListener(type, handler, false);
  } else {
    if (element.events && element.events[type]) {
      delete element.events[type][handler.$$guid];
    }
  }
}

function handleEvent(event) {
  var returnValue = true;
  event = event || fixEvent(((this.ownerDocument || this.document || this).parentWindow || window).event);
  var handlers = this.events[event.type];
  for (var i in handlers) {
    this.$$handleEvent = handlers[i];
    if (this.$$handleEvent(event) === false) {
      returnValue = false;
    }
  }
  return returnValue;
}

function fixEvent(event) {
  event.preventDefault = fixEvent.preventDefault;
  event.stopPropagation = fixEvent.stopPropagation;
  return event;
}
fixEvent.preventDefault = function() {
  this.returnValue = false;
};
fixEvent.stopPropagation = function() {
  this.cancelBubble = true;
};

if (!Array.forEach) { Array.forEach = function(array, block, context) {
    for (var i = 0; i < array.length; i++) {
      block.call(context, array[i], i, array);
    }
  };
}

Function.prototype.forEach = function(object, block, context) {
  for (var key in object) {
    if (typeof this.prototype[key] == "undefined") {
      block.call(context, object[key], key, object);
    }
  }
};

String.forEach = function(string, block, context) {
  Array.forEach(string.split(""), function(chr, index) {
    block.call(context, chr, index, string);
  });
};

var forEach = function(object, block, context) {
  if (object) {
    var resolve = Object;   if (object instanceof Function) {
      resolve = Function;
    } else if (object.forEach instanceof Function) {
      object.forEach(block, context);
      return;
    } else if (typeof object == "string") {
      resolve = String;
    } else if (typeof object.length == "number") {
      resolve = Array;
    }
    resolve.forEach(object, block, context);
  }
};