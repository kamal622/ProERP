/*
jQWidgets v4.5.2 (2017-May)
Copyright (c) 2011-2017 jQWidgets.
License: http://jqwidgets.com/license/
*/
!function(a){"use strict";a.jqx.jqxWidget("jqxKnob","",{}),a.extend(a.jqx._jqxKnob.prototype,{defineInstance:function(){var b={type:"circle",allowValueChangeOnClick:!0,allowValueChangeOnDrag:!0,allowValueChangeOnMouseWheel:!0,changing:null,dragEndAngle:-1,dragStartAngle:-1,disabled:!1,dial:{style:"transparent",innerRadius:0,outerRadius:0,gradientType:null,gradientStops:null,startAngle:null,endAngle:null},endAngle:360,height:400,labels:{type:"digits",step:null,rotate:!1,offset:null,style:"",visible:!1},marks:{type:"",thickness:1,size:"10%",colorProgress:"transparent",colorRemaining:"transparent",minorInterval:1,majorInterval:5,offset:"80%",majorSize:"15%"},min:0,max:100,progressBar:{size:"10%",offset:"60%",color:"transparent",background:"transparent"},pointer:{color:{color:"transparent",border:null,gradientType:null,gradientStops:null},thickness:1,size:"",type:"",visible:!1},pointerGrabAction:"normal",renderEngine:"",rotation:"clockwise",startAngle:0,spinner:{color:"transparent",innerRadius:0,outerRadius:0,marks:{step:1,rotate:!1,color:"transparent",size:0,steps:10,thickness:1,offset:0}},style:{fill:"transparent",stroke:"transparent"},_touchEvents:{mousedown:a.jqx.mobile.getTouchEventName("touchstart"),mouseup:a.jqx.mobile.getTouchEventName("touchend"),mousemove:a.jqx.mobile.getTouchEventName("touchmove"),mouseenter:"mouseenter",mouseleave:"mouseleave",click:a.jqx.mobile.getTouchEventName("touchstart")},step:1,snapToStep:!0,value:0,width:400};return this===a.jqx._jqxKnob.prototype?b:(a.extend(!0,this,b),b)},createInstance:function(){var b=this;b._hostInit(),b._ie8Plugin(),b._validateProperties(),b._initValues(),b._refresh(),a.jqx.utilities.resize(b.host,function(){b.widgetSize=Math.min(b.host.width(),b.host.height()),b._refresh()})},_getEvent:function(a){return this._isTouchDevice?this._touchEvents[a]+".jqxKnob"+this.element.id:a+".jqxKnob"+this.element.id},_ie8Plugin:function(){"function"!=typeof Array.prototype.forEach&&(Array.prototype.forEach=function(a){for(var b=0;b<this.length;b++)a.apply(this,[this[b],b,this])}),window.getComputedStyle||(window.getComputedStyle=function(a,b){return this.el=a,this.getPropertyValue=function(b){var c=/(\-([a-z]){1})/g;return"float"==b&&(b="styleFloat"),c.test(b)&&(b=b.replace(c,function(){return arguments[2].toUpperCase()})),a.currentStyle[b]?a.currentStyle[b]:null},this})},createColorGradient:function(a,b,c){return this._getGradient(a,b,c)},destroy:function(){var b=this;b.removeHandler(a(document),"mousemove.jqxKnob"+b.host[0].id),b.removeHandler(a(document),"blur.jqxKnob"+b.host[0].id),b.removeHandler(a(document),"mouseup.jqxKnob"+b.host[0].id),b.host.empty(),b.host.remove()},propertiesChangedHandler:function(a,b,c){c.width&&c.height&&2==Object.keys(c).length&&a._refresh()},propertyChangedHandler:function(a,b,c,d){if(!(a.batchUpdate&&a.batchUpdate.width&&a.batchUpdate.height&&2==Object.keys(a.batchUpdate).length)){if("disabled"===b&&a.host.removeClass(a.toThemeProperty("jqx-fill-state-disabled")),"value"===b)return void a._setValue(d,"propertyChange");a._validateProperties(),a._refresh()}},val:function(a){var b=this;return 0==arguments.length?b.value:void b._setValue(a,null)},_isPointerGrabbed:!1,_pointerGrabbedIndex:-1,_attatchPointerEventHandlers:function(){var b=this;b.addHandler(b.host,this._getEvent("mousedown"),function(c){if("pointer"!==b.pointerGrabAction||c.target.id===b._pointerID){if(b._isTouchDevice){var d=a.jqx.position(c);c.clientX=d.left,c.clientY=d.top}if("progressBar"===b.pointerGrabAction){var e={x:c.clientX,y:c.clientY},f=b.host[0].getBoundingClientRect(),g=b.widgetSize,h={x:f.left+g/2,y:f.top+g/2},i=b._calculateAngleFromCoordinates(e,h,b.rotation),j=b._calculateDistance(e,h);if(i<b.startAngle&&(i+=360),i>b.endAngle&&i-b.endAngle!==360+b.startAngle-i)return;var k=b._getScale(b.progressBar.offset,"w",g/2),l=b._getScale(b.progressBar.size,"w",g/2);if(j<k||j>k+l)return}return b._isPointerGrabbed=!0,b.allowValueChangeOnClick===!0&&b._mouseMovePointer(c),c.preventDefault(),c.stopPropagation(),!1}});var c=null;b.addHandler(a(document),this._getEvent("mousemove"),function(a){if(b.allowValueChangeOnDrag&&(c&&clearTimeout(c),c=setTimeout(function(){b._mouseMovePointer(a)}),b._isPointerGrabbed))return!1}),b.addHandler(a(document),"blur.jqxKnob"+b.host[0].id,function(){b._isPointerGrabbed=!1,b._pointerGrabbedIndex=-1}),b.addHandler(a(document),this._getEvent("mouseup"),function(a){b._isPointerGrabbed&&(b._isPointerGrabbed=!1,b._pointerGrabbedIndex=-1,b._raiseEvent(0,{originalEvent:a,value:b.value}))}),b.addHandler(b.host,"wheel",function(a){if(b.allowValueChangeOnMouseWheel){var c=0;return a||(a=window.event),a.originalEvent&&a.originalEvent.wheelDelta&&(a.wheelDelta=a.originalEvent.wheelDelta),a.wheelDelta?c=a.wheelDelta/120:a.detail?c=-a.detail/3:a.originalEvent&&a.originalEvent.deltaY&&(c=a.originalEvent.deltaY),c>0?b._increment():b._decrement(),!1}})},_mouseMovePointer:function(b){var c=this;if(!c.disabled&&c._isPointerGrabbed){if(c._isTouchDevice){var d=a.jqx.position(b);b.clientX=d.left,b.clientY=d.top}var e={x:b.clientX,y:b.clientY},f=c.host[0].getBoundingClientRect(),g=c.widgetSize,h={x:f.left+g/2,y:f.top+g/2},i=c._calculateAngleFromCoordinates(e,h,c.rotation),j=c._calculateValueFromAngle(i,c.dragStartAngle,c.dragEndAngle,c.min,c.max);if(c.value.length&&c._pointerGrabbedIndex===-1)for(var k=0;k<c.value.length;k++){if(j<=c.value[k]){c._pointerGrabbedIndex=k;break}if(k===c.value.length-1)c._pointerGrabbedIndex=k;else if(j<=c.value[k+1]){var l=c.value[k]+(c.value[k+1]-c.value[k])/2;c._pointerGrabbedIndex=j<=l?k:k+1;break}}if(c.pointer&&c.pointer.length>1){if(1==c._pointerGrabbedIndex){var m=c._calculateAngleFromValue(c.value[0],c.dragStartAngle,c.dragEndAngle,c.min,c.max),n=c._calculateAngleFromValue(c.max,c.dragStartAngle,c.dragEndAngle,c.min,c.max),i=c._calculateAngleFromValue(j,c.dragStartAngle,c.dragEndAngle,c.min,c.max);if(i<=m)return;if(i>=n)return}if(0==c._pointerGrabbedIndex){var n=c._calculateAngleFromValue(c.value[1],c.dragStartAngle,c.dragEndAngle,c.min,c.max),m=c._calculateAngleFromValue(c.min,c.dragStartAngle,c.dragEndAngle,c.min,c.max),i=c._calculateAngleFromValue(j,c.dragStartAngle,c.dragEndAngle,c.min,c.max);if(i<=m)return;if(i>=n)return}}if(c.changing){var o=c.value.slice(0);o[c._pointerGrabbedIndex]=j;var p=c.changing(c.value,o);if(p===!1)return}c._setValue(j,"mouse")}},_getScale:function(a,b,c){return a&&a.toString().indexOf("%")>=0?(a=parseInt(a,10)/100,"object"==typeof c?c[b]()*a:c*a):parseInt(a,10)},_hostInit:function(){var b=this;this._isTouchDevice=a.jqx.mobile.isTouchDevice();var c=b.host;c.width(b.width),c.height(b.height),c.css("position","relative"),b.host.addClass(b.toThemeProperty("jqx-widget jqx-knob")),b.dragStartAngle==-1&&(b.dragStartAngle=b.startAngle),b.dragEndAngle==-1&&(b.dragEndAngle=b.endAngle),b.dragStartAngle<b.startAngle&&(b.dragStartAngle=b.startAngle),b.dragEndAngle>b.endAngle&&(b.dragEndAngle=b.endAngle),b.widgetSize=Math.min(b.host.width(),b.host.height())},_initRenderer:function(b){if(!a.jqx.createRenderer)throw"jqxKnob: Please include a reference to jqxdraw.js";return a.jqx.createRenderer(this,b)},_initValues:function(){var a=this;a.marks&&(a.marks.style&&""!==a.marks.style&&("line"!==a.marks.style||a.marks.thickness||(a.marks.thickness=1),a.marks.size||(a.marks.size="5%"),a.marks.offset||(a.marks.offset="85%")),a.marks.majorInterval&&void 0===a.marks.majorSize&&(a.marks.majorSize="10%")),a._marksList=a._getMarksArray(a.marks),a.spinner&&(a._spinnerMarksList=a._getMarksArray(a.spinner.marks))},_calculateAngleFromValue:function(b,c,d,e,f){return a.jqx.browser.msie&&a.jqx.browser.version<9?"circle"!=this.type?e!=f?parseInt((b-e)/(f-e)):0:e!=f?parseInt((b-e)/(f-e)*(d-c)):0:"circle"!=this.type?e!=f?(b-e)/(f-e):0:e!=f?(b-e)/(f-e)*(d-c):0},_calculateAngleFromCoordinates:function(a,b,c){var d=a.x-b.x,e=a.y-b.y;return e>0?"clockwise"===c?90-180*Math.atan(d/e)/Math.PI:270+180*Math.atan(d/e)/Math.PI:e<0?"clockwise"===c?270-180*Math.atan(d/e)/Math.PI:90+180*Math.atan(d/e)/Math.PI:d>=0?0:180},_calculateValueFromAngle:function(a,b,c,d,e){a<b&&(a+=360);var f=d;return a>c?a-c<360+b-a&&(f=e):f=d+(a-b)*(e-d)/(c-b),f},_calculateDistance:function(a,b){return Math.sqrt(Math.pow(a.x-b.x,2)+Math.pow(a.y-b.y,2))},_drawBackground:function(){var a,b,c,d=this,e=d.renderer;a=d.widgetSize,b=a/2;var f=d.style.strokeWidth?d.style.strokeWidth:0;if(b-=f/2,d.style){var c=d._getColor(d.style.fill),g=d._getColor(d.style.stroke),f=d.style.strokeWidth?d.style.strokeWidth:1;"circle"!=d.type?e.rect(0,0,this.host.width(),this.host.height(),{fill:c,stroke:g,"stroke-width":f}):e.circle(a/2,a/2,b,{fill:c,stroke:g,"stroke-width":f})}},_drawDial:function(){var a=this;if(a.dial){var b,c,d,e,f,g,h,i=a.renderer,j=a.widgetSize,k=0;b=c=j/2,e=a._getScale(a.dial.outerRadius,"w",j/2),d=a._getScale(a.dial.innerRadius,"w",j/2),null!=a.dial.startAngle&&null!=a.dial.endAngle?(f="clockwise"===a.rotation?360-a.dial.endAngle:a.dial.startAngle,g="clockwise"===a.rotation?360-a.dial.startAngle:a.dial.endAngle):(f="clockwise"===a.rotation?360-a.endAngle:a.startAngle,g="clockwise"===a.rotation?360-a.startAngle:a.endAngle),h=a._getColor(a.dial.style.fill);var l=a._getColor(a.dial.style.stroke)||"",m=a.dial.style.strokeWidth||0;i.pieslice(b,c,d,e,f,g,k,{fill:h,stroke:l,"stroke-width":m})}},_getMarksArray:function(a){if(void 0==a)return[];var b,c,d=this,e={},f=d.max,g=d.min,h=f-g,i=a.minorInterval,j=a.majorInterval,k=function(a){return parseFloat(a.toPrecision(12))};if(i){for(b=0;b<h;b+=i)c=k(g+b),e[c]={type:"minor"};e[f]={type:"minor"}}if(j){for(b=0;b<h;b+=j)c=k(g+b),e[c]={type:"major"};e[f]={type:"major"}}if(!i&&!j){var l=d.step;if(l){for(b=0;b<h;b+=l)c=k(g+b),e[c]={type:"minor"};e[f]={type:"minor"}}}return e},_drawMarks:function(){var b=this;if(b.marks){var c=b.renderer,d=b.widgetSize,e=b.marks&&null!=b.marks.colorRemaining?b.marks.colorRemaining:"transparent";e=b._getColor(e),b._dialMarks=[];var f,g,h=b.marks.type;h||(h="line");var i=b._getScale(b.marks.offset,"w",d/2),j=b._marksList;a.each(j,function(j,k){if(b.dragEndAngle-b.dragStartAngle!==360||j!=b.max)if(g=b.dragStartAngle+b._calculateAngleFromValue(j,b.dragStartAngle,b.dragEndAngle,b.min,b.max),"circle"===h){var l=b._getScale(b.marks.size,"w",d/2),m=b._getPointerPosition({x:d/2,y:d/2},i,g,b.rotation);b._dialMarks.push(c.circle(m.x,m.y,l,{fill:e}))}else if("line"===h){f="major"===k.type&&null!==b.marks.majorSize&&void 0!==b.marks.majorSize?b._getScale(b.marks.majorSize,"w",d/2):b._getScale(b.marks.size,"w",d/2);var n=b._getScale(b.marks.thickness,"w",d/2),o=b._getPointerPosition({x:d/2,y:d/2},i,g,b.rotation),p=b._getPointerPosition({x:d/2,y:d/2},i+f,g,b.rotation);a.jqx.browser.msie&&a.jqx.browser.version<9?b._dialMarks.push(c.line(parseInt(o.x),parseInt(o.y),parseInt(p.x),parseInt(p.y),{stroke:e,"stroke-width":n})):b._dialMarks.push(c.line(o.x,o.y,p.x,p.y,{stroke:e,"stroke-width":n}))}})}},_drawProgressBars:function(){var b=this;if(b.progressBar){b._progressBar=b._progressBar||[];for(var c=0;c<b._progressBar.length;c++)a(b._progressBar[c]).remove();if(b._progressBar=[],b._isArray(b.progressBar.style)){var d=b.value[0],e=b.value[1],f=b.progressBar.style[0],g=b.progressBar.style[1];if(b._progressBar.push(b._drawProgressBar(b.max,b.progressBar.background,"background")),b.progressBar.ranges)for(var c=0;c<b.progressBar.ranges.length;c++){var h=b.progressBar.ranges[c].startValue,i=b.progressBar.ranges[c].endValue;b._progressBar.push(b._drawProgressBarFromToValue(h,i,b.progressBar.ranges[c],"background"))}b._progressBar.push(b._drawProgressBar(d,f)),b._progressBar.push(b._drawProgressBarFromEndToStart(e,g))}else{if(b._progressBar.push(b._drawProgressBar(b.max,b.progressBar.background,"background")),b.progressBar.ranges)for(var c=0;c<b.progressBar.ranges.length;c++){var h=b.progressBar.ranges[c].startValue,i=b.progressBar.ranges[c].endValue;b._progressBar.push(b._drawProgressBarFromToValue(h,i,b.progressBar.ranges[c],"background"))}b._progressBar.push(b._drawProgressBar(b.value,b.progressBar.style))}}},_drawProgressBarFromEndToStart:function(a,b){var c,d,e,f,g,h,i,j,k=this,l=k.renderer,m=k.widgetSize,n=k._getScale(k.progressBar.offset,"w",m/2),o=0;c=k._getScale(k.progressBar.size,"w",m/2),e=f=m/2,g=n,h=n+c;var p=k._getColor(b.fill)||"transparent",q=k._getColor(b.stroke)||"transparent";d=k.dragStartAngle+k._calculateAngleFromValue(a,k.dragStartAngle,k.dragEndAngle,k.min,k.max),i=k.dragStartAngle;var r=b.strokeWidth?b.strokeWidth:1;if(k.endAngle!=d)return"clockwise"===k.rotation?l.pieslice(e,f,g,h,360-k.endAngle,360-d,o,{fill:p,stroke:q,"stroke-width":r}):l.pieslice(e,f,g,h,j,d,o,{fill:p,stroke:q,"stroke-width":r})},_drawProgressBarFromToValue:function(a,b,c,d){var e,f,g,h,i,j,k,l=this,m=l.renderer,n=l.widgetSize,o=l._getScale(l.progressBar.offset,"w",n/2),p=0;e=l._getScale(l.progressBar.size,"w",n/2),g=h=n/2,i=o,j=o+e;var q=l._getColor(c.fill)||"transparent",r=l._getColor(c.stroke)||"transparent";if(f=l.dragStartAngle+l._calculateAngleFromValue(b,l.dragStartAngle,l.dragEndAngle,l.min,l.max),k=l.dragStartAngle+l._calculateAngleFromValue(a,l.dragStartAngle,l.dragEndAngle,l.min,l.max),k!=f){var s=1;"background"==d&&(s=0);var t=c.strokeWidth?c.strokeWidth:s;if("circle"!=l.type){if("rect"==l.type){var u=f*(this.host.height()-2*o),v=this.host.height()-2*o;return m.rect(g-e/2,o+v-u,e,u,{fill:q,stroke:r,"stroke-width":t})}return m.rect(o,h-e/2,this.host.width()-2*o,e,{fill:q,stroke:r,"stroke-width":t})}return"clockwise"===l.rotation?m.pieslice(g,h,i,j,360-f,360-k,p,{fill:q,stroke:r,"stroke-width":t}):m.pieslice(g,h,i,j,k,f,p,{fill:q,stroke:r,"stroke-width":t})}},_drawProgressBar:function(a,b,c){var d,e,f,g,h,i,j,k=this,l=k.renderer,m=k.widgetSize,n=k._getScale(k.progressBar.offset,"w",m/2),o=0;d=k._getScale(k.progressBar.size,"w",m/2),f=g=m/2,h=n,i=n+d;var p=k._getColor(b.fill)||"transparent",q=k._getColor(b.stroke)||"transparent";if(e=k.dragStartAngle+k._calculateAngleFromValue(a,k.dragStartAngle,k.dragEndAngle,k.min,k.max),j=k.dragStartAngle,j!=e){var r=1;"background"==c&&(r=0);var s=b.strokeWidth?b.strokeWidth:r;if("circle"!=k.type){if("rect"==k.type){var t=e*(this.host.height()-2*n),u=this.host.height()-2*n;return l.rect(f-d/2,n+u-t,d,t,{fill:p,stroke:q,"stroke-width":s})}return l.rect(n,g-d/2,this.host.width()-2*n,d,{fill:p,stroke:q,"stroke-width":s})}return"clockwise"===k.rotation?l.pieslice(f,g,h,i,360-e,360-j,o,{opacity:b.opacity||1,fill:p,stroke:q,"stroke-width":s}):l.pieslice(f,g,h,i,j,e,o,{opacity:b.opacity||1,fill:p,stroke:q,"stroke-width":s})}},_drawLabels:function(){var b=this;b._labels=[];var c=b.renderer,d=b.widgetSize;if(void 0===b.labels.visible&&(b.labels.visible=!0),b.labels.visible===!0){var e,f=b._getScale(b.labels.offset,"w",d/2),g=b.labels.type?b.labels.type:"digits",h=b.labels.style,i=h&&h.fill?b._getColor(h.fill):"#333";if("digits"===g){var j=[];if(b.labels.customLabels)for(e=0;e<b.labels.customLabels.length;e++)j.push(b.labels.customLabels[e].value);else{var k=b.labels.step||b.step;for(e=b.min;e<b.max;e+=k)j.push(e);b.dragEndAngle-360<b.dragStartAngle&&j.push(b.max)}for(e=0;e<j.length;e++){var l=b.labels.customLabels?b.labels.customLabels[e].text:j[e].toString();b.labels.formatFunction&&(l=b.labels.formatFunction(l));var m=b.dragStartAngle,n=b.dragEndAngle,o=m+b._calculateAngleFromValue(j[e],m,n,b.min,b.max),p=b._getPointerPosition({x:d/2,y:d/2},f,o,b.rotation);if(a.jqx.browser.msie&&a.jqx.browser.version<9){var q=c.measureText(l,0,{class:this.toThemeProperty("jqx-knob-label")}),r=b.labels.rotate?90-o:0;c.text(l,p.x-q.width/2,p.y-q.height/2,q.width,q.height,r,{class:this.toThemeProperty("jqx-knob-label")},!1)}else{var q=c.measureText(l,0,{style:{fill:i},class:this.toThemeProperty("jqx-knob-label")}),r=b.labels.rotate?90-o:0;c.text(l,p.x-q.width/2,p.y-q.height/2,q.width,q.height,r,{style:{fill:i},class:this.toThemeProperty("jqx-knob-label")},!1)}}}}},_drawPointers:function(){var b=this;if(b._pointers=b._pointers||[],b._pointers.forEach(function(b,c,d){a(b).remove(),d.splice(c,1)}),b.pointer)if(b._isArray(b.pointer))for(var c=0;c<b.progressBar.style.length;c++)b.pointer[c].visible!==!1&&b._pointers.push(b._drawPointer(b.value[c],b.pointer[c]));else{if(b.pointer.visible===!1)return;b._pointers.push(b._drawPointer(b.value,b.pointer))}},_drawPointer:function(a,b){var c=this;b.id=b.id||c._getID();var d=c.renderer,e=c.widgetSize,f=b.type;f||(f="circle"),b.style||(b.style={fill:"#feaf4e",stroke:"#feaf4e"});var g,h,i,j=c._getColor(b.style.fill),k=b.style.stroke||"",l=c._getScale(b.offset,"w",e/2),m=c.dragStartAngle+c._calculateAngleFromValue(a,c.dragStartAngle,c.dragEndAngle,c.min,c.max);if("circle"===f){var n=c._getScale(b.size,"w",e/2),o=c._getPointerPosition({x:e/2,y:e/2},l,m,c.rotation);i=d.circle(o.x,o.y,n,{id:b.id,fill:j,stroke:k})}else if("line"===f){g=c._getScale(b.size,"w",e/2),h=b.thickness;var p=c._getPointerPosition({x:e/2,y:e/2},l,m,c.rotation),q=c._getPointerPosition({x:e/2,y:e/2},l+g,m,c.rotation);i=d.line(p.x,p.y,q.x,q.y,{id:b.id,stroke:j,"stroke-width":h})}else if("arc"===f){g=c._getScale(b.size,"w",e/2);var r,s,t,u,v,w,x=0,y=(c.dragEndAngle-c.dragStartAngle)/c._steps.length;r=s=e/2,t=l,u=l+g,v="clockwise"===c.rotation?360-(m+y/2):m-y/2,w="clockwise"===c.rotation?360-(m-y/2):m+y/2,i=d.pieslice(r,s,t,u,v,w,x,{id:b.id,fill:j,stroke:k})}else if("arrow"===f){g=c._getScale(b.size,"w",e/2),h=b.thickness;var z=c._getPointerPosition({x:e/2,y:e/2},g,m,c.rotation),A=c._getPointerPosition({x:e/2,y:e/2},l,m,c.rotation),B=c._getPointerPosition({x:A.x,y:A.y},h/2,m-90,c.rotation),C=c._getPointerPosition({x:A.x,y:A.y},h/2,m+90,c.rotation),D="M "+z.x+","+z.y+" L "+B.x+","+B.y+" L "+C.x+","+C.y+" "+z.x+","+z.y;i=this.renderer.path(D,{id:b.id,stroke:k,fill:j})}return i},_rotateSpinnerMarks:function(b){var c=this,d=c.spinner.marks;if(d){if(d.rotate===!1)return;var e=c.renderer,f=c.widgetSize,g=d&&null!=d.colorRemaining?d.colorRemaining:"transparent";g=c._getColor(g);var h,i,j=d.type;j||(j="line");for(var k=c._getScale(d.offset,"w",f/2),l=0;l<c._spinnerMarks.length;l++)a(c._spinnerMarks[l]).remove();c._spinnerMarks=[];var m=c._spinnerMarksList;a.each(m,function(a,l){if(c.endAngle-c.startAngle!==360||a!=c.max){if(i=b+c._calculateAngleFromValue(a,c.startAngle,c.endAngle,c.min,c.max),i<c.startAngle)return!0;if(i>c.endAngle&&i<c.startAngle+360)return!0;if("circle"===j){var m=c._getScale(d.size,"w",f/2),n=c._getPointerPosition({x:f/2,y:f/2},k,i,c.rotation);c._spinnerMarks.push(e.circle(n.x,n.y,m,{fill:g}))}else if("line"===j){h="major"===l.type&&null!==d.majorSize&&void 0!==d.majorSize?c._getScale(d.majorSize,"w",f/2):c._getScale(d.size,"w",f/2);var o=c._getScale(d.thickness,"w",f/2),p=c._getPointerPosition({x:f/2,y:f/2},k,i,c.rotation),q=c._getPointerPosition({x:f/2,y:f/2},k+h,i,c.rotation);c._spinnerMarks.push(e.line(p.x,p.y,q.x,q.y,{stroke:g,"stroke-width":o}))}}})}},_drawSpinnerMarks:function(b){var c=this;if(b){var d=c.renderer,e=c.widgetSize,f=b&&null!=b.colorRemaining?b.colorRemaining:"transparent";f=c._getColor(f),c._spinnerMarks=[];var g,h,i=b.type;i||(i="line");var j=c._getScale(b.offset,"w",e/2),k=c._spinnerMarksList;a.each(k,function(a,k){if(c.dragEndAngle-c.dragStartAngle!==360||a!=c.max)if(h=c.startAngle+c._calculateAngleFromValue(a,c.startAngle,c.endAngle,c.min,c.max),"circle"===i){var l=c._getScale(b.size,"w",e/2),m=c._getPointerPosition({x:e/2,y:e/2},j,h,c.rotation);c._spinnerMarks.push(d.circle(m.x,m.y,l,{fill:f}))}else if("line"===i){g="major"===k.type&&null!==b.majorSize&&void 0!==b.majorSize?c._getScale(b.majorSize,"w",e/2):c._getScale(b.size,"w",e/2);var n=c._getScale(b.thickness,"w",e/2),o=c._getPointerPosition({x:e/2,y:e/2},j,h,c.rotation),p=c._getPointerPosition({x:e/2,y:e/2},j+g,h,c.rotation);c._spinnerMarks.push(d.line(o.x,o.y,p.x,p.y,{stroke:f,"stroke-width":n}))}})}},_drawSpinner:function(){var a=this;if(a.spinner){var b=a.renderer,c=a.widgetSize;a.spinner.style||(a.spinner.style={fill:"#dfe3e9",stroke:"#dfe3e9"});var d,e,f=a._getColor(a.spinner.style.fill),g=a.spinner.style.stroke||"";d=e=c/2;var h=a._getScale(a.spinner.outerRadius,"w",c/2),i=a._getScale(a.spinner.innerRadius,"w",c/2),j=f.strokeWidth?f.strokeWidth:2;if(b.pieslice(d,e,i,h,360-a.endAngle,360-a.startAngle,0,{"stroke-width":j,fill:f,stroke:g}),a.spinner.marks){return void a._drawSpinnerMarks(a.spinner.marks)}}},_getColor:function(a){return a&&"object"==typeof a?this._getGradient(a.color,a.gradientType,a.gradientStops):a},_getGradient:function(a,b,c){return b&&null!=c&&"object"==typeof c&&("linear"===b?a=this.renderer._toLinearGradient(a,!0,c):"linearHorizontal"===b?a=this.renderer._toLinearGradient(a,!1,c):"radial"===b&&(a=this.renderer._toRadialGradient(a,c))),a},_isArray:function(a){return"[object Array]"===Object.prototype.toString.call(a)},_events:["slide","change"],_raiseEvent:function(b,c){var d=this._events[b],e=a.Event(d);return e.args=c,this.host.trigger(e)},_movePointers:function(){for(var a,b=this,c=0;c<b._pointers.length;c++)1!==b._pointers.length?(a=b.dragStartAngle+b._calculateAngleFromValue(b.value[c],b.dragStartAngle,b.dragEndAngle,b.min,b.max),b._pointers[c]=b._movePointer(b._pointers[c],b.pointer[c],a,b.value[c])):(a=b.dragStartAngle+b._calculateAngleFromValue(b.value,b.dragStartAngle,b.dragEndAngle,b.min,b.max),b._pointers[0]=b._movePointer(b._pointers[0],b.pointer,a,b.value))},_movePointer:function(b,c,d,e){var f,g=this,h=g.renderer,i=g.widgetSize,j=c.type;j||(j="circle");var k=g._getScale(c.offset,"w",i/2);if("circle"===j){var l=g._getPointerPosition({x:i/2,y:i/2},k,d,g.rotation);h.attr(b,{cx:l.x,cy:l.y}),a.jqx.browser.msie&&a.jqx.browser.version<9&&(a("#"+c.id).remove(),b=g._drawPointer(e,c))}else if("line"===j){f=g._getScale(c.size,"w",i/2);var m=g._getPointerPosition({x:i/2,y:i/2},k,d,g.rotation),n=g._getPointerPosition({x:i/2,y:i/2},k+f,d,g.rotation);h.attr(b,{x1:m.x,y1:m.y,x2:n.x,y2:n.y}),a.jqx.browser.msie&&a.jqx.browser.version<9&&(a("#"+c.id).remove(),b=g._drawPointer(e,c))}else if("arrow"===j){f=g._getScale(c.size,"w",i/2);var o=c.thickness,p=g._getPointerPosition({x:i/2,y:i/2},f,d,g.rotation),q=g._getPointerPosition({x:i/2,y:i/2},k,d,g.rotation),r=g._getPointerPosition({x:q.x,y:q.y},o/2,d-90,g.rotation),s=g._getPointerPosition({x:q.x,y:q.y},o/2,d+90,g.rotation),t="M "+p.x+","+p.y+" L "+r.x+","+r.y+" L "+s.x+","+s.y+" "+p.x+","+p.y;h.attr(b,{d:t}),a.jqx.browser.msie&&a.jqx.browser.version<9&&(a("#"+c.id).remove(),b=g._drawPointer(e,c))}else"arc"===j&&(a("#"+c.id).remove(),b=g._drawPointer(c));return g.progressBar&&b.parentNode.appendChild(b.parentNode.removeChild(b)),b},_getPointerPosition:function(b,c,d,e){return a.jqx.browser.msie&&a.jqx.browser.version<9?{x:parseInt(b.x+c*Math.sin(Math.PI/180*(d+90))),y:"clockwise"===e?parseInt(b.y+c*Math.sin(Math.PI/180*d)):parseInt(b.y-c*Math.sin(Math.PI/180*d))}:{x:b.x+c*Math.sin(Math.PI/180*(d+90)),y:"clockwise"===e?b.y+c*Math.sin(Math.PI/180*d):b.y-c*Math.sin(Math.PI/180*d)}},_getID:function(){var a=function(){return 16*(1+Math.random())|0};return""+a()+a()+"-"+a()+"-"+a()+"-"+a()+"-"+a()+a()+a()},_decrement:function(){this._setValue(this.value-this.step,"mouse")},_increment:function(){this._setValue(this.value+this.step,"mouse")},_refresh:function(){var b=this;b.disabled&&b.host.addClass(b.toThemeProperty("jqx-fill-state-disabled")),b.renderer||(b._isVML=!1,b.host.empty(),b._initRenderer(b.host)),b.removeHandler(a(document),"mousemove.jqxKnob"+b.host[0].id),b.removeHandler(a(document),"blur.jqxKnob"+b.host[0].id),b.removeHandler(a(document),"mouseup.jqxKnob"+b.host[0].id),b.removeHandler(b.host,"wheel"),b.removeHandler(b.host,"mousedown"),b.host.empty(),b._initRenderer(b.host);var c=b.renderer;if(c){b._steps=[];for(var d=0;d<=(b.max-b.min)/b.step;d++)b._steps.push(b.min+b.step*d);b._initValues(),b._render()}},_render:function(){var a=this;a._drawBackground(),a._drawDial(),a._drawMarks(),a._drawLabels(),a._drawSpinner(),a._drawProgressBars(),a._updateMarksColor(),a._updateSpinnerMarksColor(),a._drawPointers(),a._attatchPointerEventHandlers()},_setValue:function(b,c){var d=this,e=d.value;if(isNaN(b)&&(b=d.min),b>d.max?b=d.max:b<d.min&&(b=d.min),d.snapToStep)for(var f=d._steps,g=0;g<f.length;g++)if(b<f[g]){b=0===g?f[g]:f[g]-b<b-f[g-1]?f[g]:f[g-1];break}if(b!=e){if(a.isArray(d.value)){if(d._pointerGrabbedIndex!=-1){if(1==d._pointerGrabbedIndex){d.value[0];d.value[d._pointerGrabbedIndex]=b}if(0==d._pointerGrabbedIndex){d.value[1];d.value[d._pointerGrabbedIndex]=b}d.value[d._pointerGrabbedIndex]=b}}else d.value=b;d._updateProgressBarColor(),d._updateMarksColor(),d._updateSpinnerMarksColor();var h=d.dragStartAngle+d._calculateAngleFromValue(b,d.dragStartAngle,d.dragEndAngle,d.min,d.max);d._rotateSpinnerMarks(h),d._movePointers(),d._raiseEvent(1,{value:d.value,type:c})}},_updateMarksColor:function(){var b=this;if(b.marks&&(b.marks.colorProgress||b.marks.colorRemaining)){var c=b.renderer,d=[];a.each(b._marksList,function(a){return b.endAngle-b.startAngle===360&&a==b.max?void d.push(a):void d.push(a)});for(var e=b._getColor(b.marks.colorProgress),f=b._getColor(b.marks.colorRemaining),g=b.value.length?b.value[0]:b.value,h=0;h<b._dialMarks.length;h++)d[h]>g?"circle"===b.marks.type?c.attr(b._dialMarks[h],{fill:f}):c.attr(b._dialMarks[h],{stroke:f}):"circle"===b.marks.type?c.attr(b._dialMarks[h],{fill:e}):c.attr(b._dialMarks[h],{stroke:e}),b.progressBar&&b.marks.drawAboveProgressBar&&b._dialMarks[h].parentNode.appendChild(b._dialMarks[h].parentNode.removeChild(b._dialMarks[h]))}},_updateSpinnerMarksColor:function(){var b=this;if(b.spinner&&b.spinner.marks&&b.spinner.marks&&(b.spinner.marks.colorProgress||b.spinner.marks.colorRemaining)){var c=b.renderer,d=[];a.each(b._spinnerMarksList,function(a){b.endAngle-b.startAngle===360&&a==b.max||d.push(a)});for(var e=b._getColor(b.spinner.marks.colorProgress),f=b._getColor(b.spinner.marks.colorRemaining),g=b.value.length?b.value[0]:b.value,h=0;h<b._spinnerMarks.length;h++)d[h]>g?"circle"===b.spinner.marks.type?c.attr(b._spinnerMarks[h],{fill:f}):c.attr(b._spinnerMarks[h],{stroke:f}):"circle"===b.spinner.marks.type?c.attr(b._spinnerMarks[h],{fill:e}):c.attr(b._spinnerMarks[h],{stroke:e})}},_updateProgressBarColor:function(){var a=this;a.progressBar&&a._drawProgressBars()},_validateProperties:function(){var a=this,b=function(a,b){if(a&&"string"==typeof a){var c=a;return a={fill:c,stroke:c}}return a||(a={},a.fill=b,a.stroke=b),a&&a.fill&&!a.stroke&&(a.stroke=a.fill),a&&!a.fill&&a.stroke&&(a.fill=a.stroke),a&&!a.fill&&(a.fill=b),a&&!a.stroke&&(a.stroke=b),a};if(a.dial&&(a.dial.style=b(a.dial.style,"#dddddd")),a.style&&(a.style=b(a.style,"#dddddd")),a.progressBar&&(a.progressBar.style=b(a.progressBar.style,"transparent"),a.progressBar.background=b(a.progressBar.background,"transparent")),a.spinner&&(a.spinner.style=b(a.spinner.style,"transparent")),a.pointer&&(a.pointer.style=b(a.pointer.style,"transparent")),a.startAngle>=a.endAngle)throw new Error("jqxKnob: The end angle must be bigger than the start angle!");if(a.startAngle<0||a.startAngle>360)throw new Error("jqxKnob: Start angle must be between 0 and 360");if(a.endAngle>a.startAngle+360)throw new Error("jqxKnob: End angle must be between startAngle and startAngle + 360");if(a.dial&&a.dial.color&&"transparent"!==a.dial.color&&(!a.dial.outerRadius||!a.dial.innerRadius))throw new Error("jqxKnob: Dial options innerRadius and outerRadius need to be specified");if(a._isArray(a.pointer)||a._isArray(a.value)){if(!a._isArray(a.pointer))throw new Error("jqxKnob: If the value is an array, the pointer must also be an array.");if(!a._isArray(a.value))throw new Error("jqxKnob: If the pointer is an array, the value must also be an array.");if(a.pointer.length!==a.value.length)throw new Error("jqxKnob: The pointer and value array sizes must match.");if(a.progressBar&&(!a._isArray(a.progressBar.style)||a.progressBar.style.length!==a.pointer.length))throw new Error("jqxKnob: progressBar color must be an array with the same number of elements as the pointer and value.")}return!0}})}(jqxBaseFramework);

