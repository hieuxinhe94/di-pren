define("epi/shell/logger", [], function () {
    // summary:
    //  Use to add console logging in a handled manner that can be used even
    //  with built versions of the client modules.
    //
    // example:
    //      |   var handle = logger.timedGroup("myLabel");
    //      |   // do some operations
    //      |   handle.end()

    var noop = function () { },
        c = console,
        uid = 1;

    function timedGroup(label) {

        var id = uid++;

        label = id + ": " + label;

        c.group(label);
        c.time(id);

        return {
            end: function () {
                c.timeEnd(id);
                c.groupEnd(label);
                id = label = null;
            }
        };
    }

    function setConsole(value) {
        c = value;
    }

    if (!c || !c.group || !c.time) {
        c = {
            "log": noop,
            "time": noop,
            "timeEnd": noop,
            "group": noop,
            "groupEnd": noop
        };
    }

    return {
        log: function () {
            c.log.apply(c, arguments);
        },
        time: function () {
            c.time.apply(c, arguments);
        },
        timeEnd: function () {
            c.timeEnd.apply(c, arguments);
        },
        group: function () {
            c.group.apply(c, arguments);
        },
        groupEnd: function () {
            c.groupEnd.apply(c, arguments);
        },
        timedGroup: timedGroup,
        setConsole: setConsole
    };
});
