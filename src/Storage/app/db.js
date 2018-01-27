function DynamoDB() {
    var AWS = require('aws-sdk');
    AWS.config.loadFromPath('./config.json');

    this.dynamodb = new AWS.DynamoDB();
    this.docClient = new AWS.DynamoDB.DocumentClient();
    this.table = "documents";
}

DynamoDB.prototype.create = function () {
    var params = {
        TableName: this.table,
        KeySchema: [{
                AttributeName: "key",
                KeyType: "HASH"
            }, //Partition key
            {
                AttributeName: "title",
                KeyType: "RANGE"
            } //Sort key
        ],
        AttributeDefinitions: [{
                AttributeName: "key",
                AttributeType: "S"
            },
            {
                AttributeName: "title",
                AttributeType: "S"
            }
        ],
        ProvisionedThroughput: {
            ReadCapacityUnits: 10,
            WriteCapacityUnits: 10
        }
    };

    this.dynamodb.createTable(params, function (err, data) {
        if (err) {
            console.error("Unable to create table. Error JSON:", JSON.stringify(err, null, 2));
            return false;
        } else {
            console.log("Created table. Table description JSON:", JSON.stringify(data, null, 2));
            return data;
        }
    });
};

DynamoDB.prototype.select = function (object) {
    var params = {
        TableName: this.table,
        Key: {
            "key": object.key,
            "title": object.title
        }
    };

    this.docClient.get(params, function (err, data) {
        if (err) {
            console.error("Unable to read item. Error JSON:", JSON.stringify(err, null, 2));
            return false;
        } else {
            console.log("GetItem succeeded:", JSON.stringify(data, null, 2));
            return data;
        }
    });
};

DynamoDB.prototype.insert = function (object) {
    var params = {
        TableName: this.table,
        Item: {
            "key": object.key,
            "title": object.title,
            "info": {
                "lang": object.lang,
                "create": new Date().toString(),
                "edit": new Date().toString(),
            }
        }
    };

    this.docClient.put(params, function (err, data) {
        if (err) {
            console.error("Unable to add object", object.title, ". Error JSON:", JSON.stringify(err, null, 2));
            return false;
        } else {
            console.log("PutItem succeeded:", object.title);
            return data;
        }
    });
};

DynamoDB.prototype.update = function (object) {
    var params = {
        TableName: this.table,
        Key:{
            "key": object.key,
            "title": object.title
        },
        UpdateExpression: "set info.lang=:r, info.edit=:p",
        ExpressionAttributeValues:{
            ":r": object.lang,
            ":p": new Date().toString()
        },
        ReturnValues:"UPDATED_NEW"
    };

    this.docClient.update(params, function(err, data) {
        if (err) {
            console.error("Unable to update item. Error JSON:", JSON.stringify(err, null, 2));
            return false;
        } else {
            console.log("UpdateItem succeeded:", JSON.stringify(data, null, 2));
            return data;
        }
    });
};

DynamoDB.prototype.delete = function (object) {
    var params = {
        TableName: this.table,
        Key:{
            "key":object.key,
            "title":object.title
        },
    };
    
    this.docClient.delete(params, function(err, data) {
        if (err) {
            console.error("Unable to delete item. Error JSON:", JSON.stringify(err, null, 2));
            return false;
        } else {
            console.log("DeleteItem succeeded:", JSON.stringify(data, null, 2));
            return data;
        }
    });
};

DynamoDB.create = function () {
    return new DynamoDB();
}


/*
var object = {
    key: "testkey2",
    title: "testtitle2",
    lang: "fr"
};

var run = DynamoDB.create();
// run.create();
// run.insert(object);
// run.select(object);
// run.update(object);
// run.delete(object);
*/