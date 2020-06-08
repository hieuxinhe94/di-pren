define("epi/shell/store/Registry", [
    "dojo",
    "dojo/store/Memory",
    "epi/shell/store/JsonRest",
    "epi/shell/store/Patchable",
    "epi/shell/store/Throttle"
], function (dojo, Memory, JsonRest, Patchable, Throttle) {

    return dojo.declare(null, {
        // summary:
        //		Implementation of a store registry used for handling store sharing between different components.
        // tags:
        //		public

        constructor: function (options) {
            dojo.mixin(this, options);
            this._stores = {};
        },

        add: function (name, store) {
            // summary:
            //		Adds a store to the collection with the given name. This will throw an
            //		error if the name already exists in the collection.
            // name: String
            //		The name to be associated with the store.
            // store: Object
            //		The store that will be added.

            if (this._stores[name]) {
                throw new Error("A store with the name '" + name + "' already exists.");
            }
            return (this._stores[name] = store);
        },

        create: function (name, url, options) {
            // summary:
            //		Creates an observable JsonRest store with a backing cache store and
            //		adds it to the collection with the given name. This will throw an error
            //		if the name already exists in the collection.
            // name: String
            //		The name to be associated with the store.
            // url: String
            //		The url to be used as the JsonRest store target.
            // options: Object
            //		This provides any configuration information that will be mixed into the store.

            if (this._stores[name]) {
                throw new Error("A store with the name '" + name + "' already exists.");
            }

            var memory = new Memory(options),
                rest = new JsonRest(dojo.mixin({
                    target: url,
                    preventCache: true
                }, options)),

                // Allow only one concurrent get to the server for the same data.
                patchable = Patchable(Throttle(rest, "get"), memory),

                // The Observable isn't happy about several concurrent query calls.
                store = Throttle(patchable, "query");

            return (this._stores[name] = store);
        },

        get: function (name) {
            // summary:
            //		Get a store by its registered name.
            // name: String
            //		The name of the store to return.

            var store = this._stores[name];
            if (!store) {
                throw new Error("No store by the name '" + name + "' exists.");
            }
            return store;
        }
    });
});
