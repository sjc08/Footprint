window._AMapSecurityConfig = {
    securityJsCode: 'c68019763999486713f65af69b812d37'
}

var scene;
switch (CS.Map()) {
    case 'amap':
        scene = new L7.Scene({
            id: 'map',
            map: new L7.GaodeMap({
                style: CS.Style(),
                token: '3b9a8c37d8a26809a658bb8c59ce17a4'
            })
        });
        break;
    case 'mapbox':
        scene = new L7.Scene({
            id: 'map',
            map: new L7.Mapbox({
                style: 'blank'
            })
        });
        switch (CS.Style()) {
            case 'outline':
                $.getJSON('world.geo.json', d => {
                    const mapLayer = new L7.LineLayer()
                        .source(d)
                        .color('gray')
                        .size(0.3)
                    scene.addLayer(mapLayer);
                });
                break;
        }
        break;
}