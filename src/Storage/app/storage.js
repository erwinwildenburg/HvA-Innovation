function Storage() {
    var AWS = require('aws-sdk');
    AWS.config.loadFromPath('./config.json');

    this.s3 = new AWS.S3();
    this.bucket = 'innovation-document-bucket';
}

Storage.prototype.createBucket = function () {
    this.s3.createBucket({
        Bucket: this.bucket
    }, function (err, data) {
        if (err) {
            console.error("Unable to create bucket. Error JSON:", JSON.stringify(err, null, 2));
            return false;
        } else {
            console.log("Create bucket succeeded:", JSON.stringify(data, null, 2));
            return data;
        }
    });
};

Storage.prototype.get = function (object) {
    this.s3.getObject({
        Bucket: this.bucket,
        Key: object.key
    }, function (err, data) {
        if (err) {
            console.error("Unable to get object. Error JSON:", JSON.stringify(err, null, 2));
            return false;
        } else {
            console.log("Get object succeeded:", JSON.stringify(data, null, 2));
            return data;
        }
    });
};

Storage.prototype.put = function (object) {
    var params = {
        Bucket: this.bucket,
        Key: object.key,
        Body: object.content
    };

    this.s3.putObject(params, function (err, data) {
        if (err) {
            console.error("Unable to store object. Error JSON:", JSON.stringify(err, null, 2));
            return false;
        } else {
            console.log("Store object succeeded:", JSON.stringify(data, null, 2));
            return data;
        }
    });
};

Storage.prototype.update = function (object) {
    return this.put(object);
};

Storage.prototype.delete = function (object) {
    var params = {
        Bucket: this.bucket,
        Key: object.key
    };

    this.s3.deleteObject(params, function (err, data) {
        if (err) {
            console.error("Unable to delete object. Error JSON:", JSON.stringify(err, null, 2));
            return false;
        } else {
            console.log("Delete object succeeded:", JSON.stringify(data, null, 2));
            return data;
        }
    });
};

Storage.create = function () {
    return new Storage();
}

/*
var object = {
    key: "testkey",
    content: "Body of the file"
};

var run = Storage.create();
// run.createBucket();
// run.put(object);
// run.get(object);
// run.update(object);
// run.delete(object);
*/