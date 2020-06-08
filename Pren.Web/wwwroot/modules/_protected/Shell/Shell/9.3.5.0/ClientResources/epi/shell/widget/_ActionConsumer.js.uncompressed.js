define("epi/shell/widget/_ActionConsumer", [
"dojo",
"dojo/_base/kernel",
"epi/shell/widget/_ActionProviderBase"
],

function (dojo, kernel, _ActionProviderBase) {

    return dojo.declare(_ActionProviderBase, {
        // summary:
        //    Mixin for components that are responsible for rendering controls and executing custom actions provided by action providers.
        //
        // tags:
        //    deprecated

        // _actionProviders: [private] Array
        //    An array of action providers used by this instance to get and handle the list of custom actions.
        _actionProviders: null,

        constructor: function (args) {
            kernel.deprecated("epi/shell/widget/_ActionConsumer", "use the command pattern with epi/shell/command/_CommandConsumerMixin instead");

            this._actionProviders = [];
            if (typeof (args) !== 'undefined' || args != null && dojo.isArray(args.actionProviders)) {
                dojo.forEach(args.actionProviders, function (provider) {
                    this.addProvider(provider);
                }, this);
            }
        },

        isProviderRegistered: function (actionProvider) {
            // summary:
            //    Returns a boolean value indicating whether specified action provider is registered in this action consumer.
            //
            // actionProvider:
            //      An action provider to verify.
            //
            // tags:
            //    public

            if (typeof (actionProvider) === 'undefined' || actionProvider == null) {
                return false;
            }
            return dojo.some(this._actionProviders, function (providerStruct) {
                return providerStruct.provider === actionProvider;
            });
        },

        addProvider: function (actionProvider) {
            // summary:
            //    Registers specified action provider for this action consumer.
            //
            // actionProvider:
            //      An action provider to add.
            //
            // tags:
            //    public

            if (typeof (actionProvider) === 'undefined' || actionProvider == null || this.isProviderRegistered(actionProvider)) {
                return;
            }
            this._actionProviders.push({
                provider: actionProvider,
                connects: [
                    dojo.connect(actionProvider, "onActionAdded", this, "onActionAdded"),
                    dojo.connect(actionProvider, "onActionRemoved", this, "onActionRemoved"),
                    dojo.connect(actionProvider, "onActionPropertyChanged", this, "onActionPropertyChanged")]
            });
        },

        removeProvider: function (actionProvider) {
            // summary:
            //    Removes specified action provider from the list of this action consumer.
            //
            // actionProvider:
            //      An action provider to remove.
            //
            // tags:
            //    public

            if (typeof (actionProvider) === 'undefined' || actionProvider == null || !this.isProviderRegistered(actionProvider)) {
                return;
            }
            for (var i = 0; i < this._actionProviders.length; i++) {
                if (this._actionProviders[i].provider == actionProvider) {
                    var provider = this._actionProviders.splice(i, 1);
                    dojo.forEach(provider.connects, dojo.disconnect);
                    break;
                }
            }
        },

        getActions: function () {
            // summary:
            //    Returns an array of custom actions provided by all action providers in actionProviders array.
            //    Returns the empty array if action providers are not defined or don't provide any actions.
            //
            // tags:
            //    protected

            var actions = [];
            dojo.forEach(this._actionProviders, function (provider, index) {
                actions = actions.concat(this._getProviderActions(provider.provider));
            }, this);
            return actions;
        },

        _getProviderActions: function (actionProvider) {
            // summary:
            //    Returns an array of custom actions provided by specified action provider.
            //    Returns the empty array if action provider is null or not defined or does not provide any action.
            //
            // actionProvider:
            //      An action provider to get actions from.
            //
            // tags:
            //    private

            if (typeof (actionProvider) !== 'undefined' && actionProvider != null && typeof (actionProvider.getActions) == "function") {
                return actionProvider.getActions();
            } else {
                return [];
            }
        }
    });
});
