window.bcOnInvoke = {
  invoke: function (id, dotnetObj) {
    console.log(dotnetObj);
    document.getElementById(id).addEventListener("slid.bs.carousel",
      (e) => {
        console.log(e);
        dotnetObj.invokeMethodAsync("MyCallbackFunction", `${e.to}`);
      });
  }
}