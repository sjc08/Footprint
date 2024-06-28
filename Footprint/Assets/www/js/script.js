window._AMapSecurityConfig = {
    securityJsCode: 'c68019763999486713f65af69b812d37'
}

var scene;
switch (CS.Map().split('.')[0]) {
    case 'AMap':
        scene = new L7.Scene({
            id: 'map',
            map: new L7.GaodeMap({
                style: CS.Map().split('.')[1],
                token: '3b9a8c37d8a26809a658bb8c59ce17a4'
            })
        });
        break;
    case 'Offline':
        scene = new L7.Scene({
            id: 'map',
            map: new L7.Mapbox({
                style: 'blank'
            })
        });
        switch (CS.Map().split('.')[1]) {
            case 'colorful':
                Promise.all([
                    fetch('world.geo.json').then(d => d.json()),
                    fetch('national-colors.json').then(d => d.json())
                ]).then(function onLoad([world, colors]) {
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