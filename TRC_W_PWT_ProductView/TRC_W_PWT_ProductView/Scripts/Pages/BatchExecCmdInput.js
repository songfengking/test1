///////////////////////////////////////////////////////////////////////////////////////////////
//ユーザJOB起動画面(BatchExecCmdInput)用制御関数
///////////////////////////////////////////////////////////////////////////////////////////////
//メイン処理からの各画面派生呼び出し(PageLoad)
function MasterMainPageLoaded() {
}
//メイン処理からの各画面派生呼び出し(Resize)
function MasterMainPageResized() {
}

BatchExecCmdInput = {
    //定数
    CHK_PRODUCT: "MasterBody_chkProduct"
    ,
    DT_PRODUCT_FROM: "MasterBody_cldProductFrom"
    ,
    DT_PRODUCT_TO: "MasterBody_cldProductTo"
    ,
    CHK_STOCK: "MasterBody_chkStock"
    ,
    DT_STOCK_FROM: "MasterBody_cldStockFrom"
    ,
    DT_STOCK_TO: "MasterBody_cldStockTo"
    ,
    CHK_SHIPMENT: "MasterBody_chkShipment"
    ,
    DT_SHIPMENT_FROM: "MasterBody_cldShipmentFrom"
    ,
    DT_SHIPMENT: "MasterBody_cldShipmentTo"
    ,
    CheckChanged: function (evt) {

        //生産中
        var chkProduct = $("#" + this.CHK_PRODUCT)[0];
        var dtProductFrom = $("#" + this.DT_PRODUCT_FROM);
        var dtProductTo = $("#" + this.DT_PRODUCT_TO);

        //在庫
        var chkStock = $("#" + this.CHK_STOCK)[0];
        var dtStockFrom = $("#" + this.DT_STOCK_FROM);
        var dtStockTo = $("#" + this.DT_STOCK_TO);

        //出荷
        var chkShipment = $("#" + this.CHK_SHIPMENT)[0];
        var dtShipmentFrom = $("#" + this.DT_SHIPMENT_FROM);
        var dtShipmentTo = $("#" + this.DT_SHIPMENT);

        //生産中
        if (chkProduct.checked) {
            $(dtProductFrom)[0].disabled = "";
            $(dtProductTo)[0].disabled = "";

        } else {
            $(dtProductFrom)[0].disabled = "true";
            $(dtProductTo)[0].disabled = "true";
        }

        //在庫
        if (chkStock.checked) {
            $(dtStockFrom)[0].disabled = "";
            $(dtStockTo)[0].disabled = "";

        } else {
            $(dtStockFrom)[0].disabled = "true";
            $(dtStockTo)[0].disabled = "true";
        }

        //出荷
        if (chkShipment.checked) {
            $(dtShipmentFrom)[0].disabled = "";
            $(dtShipmentTo)[0].disabled = "";

        } else {
            $(dtShipmentFrom)[0].disabled = "true";
            $(dtShipmentTo)[0].disabled = "true";
        }

        return true;
    }
}