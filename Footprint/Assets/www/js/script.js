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
    case 'offline':
        scene = new L7.Scene({
            id: 'map',
            map: new L7.Mapbox({
                style: 'blank'
            })
        });
        switch (CS.Style()) {
            case 'colorful':
                var world, colors;
                $.when(
                    $.getJSON('world.geo.json', d => world = d),
                    $.getJSON('national-colors.json', d => colors = d)
                ).then(() => {
                    const polygonLayer = new L7.PolygonLayer()
                        .source(world)
                        .color('name', value => {
                            for (const i of colors) {
                                if (i.country.toLowerCase() == value.toLowerCase())
                                    return i.colors.primary[0];
                            }
                            return 'gray';
                        })
                        .shape('fill')
                    scene.addLayer(polygonLayer);
                    const lineLayer = new L7.LineLayer()
                        .source(world)
                        .color('gray')
                        .size(0.3)
                    scene.addLayer(lineLayer);
                });
                break;
        }
        break;
}