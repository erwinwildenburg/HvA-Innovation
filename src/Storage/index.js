var express = require('express');
var bodyParser = require('body-parser')

var db = require('./app/db.js');
var storage = require('./app/storage.js');

var app = express();

app.use(bodyParser.json()); // to support JSON-encoded bodies
app.use(bodyParser.urlencoded({ // to support URL-encoded bodies
    extended: true
}));


app.post('/getAllFiles', function (req, res) {
    db.selectAll().then(function (data) {
        res.json(data);
    }, function (err) {
        res.json(err);
    });
});

app.post('/getFile', function (req, res) {
    const reqData = req.body;
    const params = {
        key: reqData.key
    }

    storage.get(params).then(function (data) {
        res.json(data);
    }, function (err) {
        res.json(err);
    });
});

app.post('/storeFile', function (req, res) {
    const reqData = req.body;
    const params = {
        key: reqData.key,
        title: reqData.title,
        lang: reqData.lang,
        content: reqData.content
    }

    db.update(params).then(function () {
        storage.put(params).then(function (data) {
            res.json(data);
        }, function (err) {
            res.json(err);
        });
    }, function (err) {
        res.json(err);
    });
});

app.post('/createFile', function (req, res) {
    const reqData = req.body;
    const params = {
        key: reqData.key,
        title: reqData.title
    }

    db.insert(params).then(function (data) {
        res.json(data);
    }, function (err) {
        res.json(err);
    });
});

app.post('/deleteFile', function (req, res) {
    const reqData = req.body;
    const params = {
        key: reqData.key,
        title: reqData.title
    }

    db.delete(params).then(function () {
        storage.delete(params).then(function (data) {
            res.json(data);
        }, function (err) {
            res.json(err);
        });
    })
});

var server = app.listen(8081, function () {
    var host = server.address().address
    var port = server.address().port

    console.log("Node.js listening at http://%s:%s", host, port)
})