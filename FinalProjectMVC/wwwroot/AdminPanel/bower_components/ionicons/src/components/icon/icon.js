var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Build, Component, Element, Host, Prop, State, Watch, h } from '@stencil/core';
import { getSvgContent, ioniconContent } from './request';
import { getName, getUrl, inheritAttributes, isRTL } from './utils';
let Icon = class Icon {
    constructor() {
        this.iconName = null;
        this.inheritedAttributes = {};
        this.isVisible = false;
        /**
         * The mode determines which platform styles to use.
         */
        this.mode = getIonMode();
        /**
         * If enabled, ion-icon will be loaded lazily when it's visible in the viewport.
         * Default, `false`.
         */
        this.lazy = false;
        /**
         * When set to `false`, SVG content that is HTTP fetched will not be checked
         * if the response SVG content has any `<script>` elements, or any attributes
         * that start with `on`, such as `onclick`.
         * @default true
         */
        this.sanitize = true;
    }
    componentWillLoad() {
        this.inheritedAttributes = inheritAttributes(this.el, ['aria-label']);
    }
    connectedCallback() {
        // purposely do not return the promise here because loading
        // the svg file should not hold up loading the app
        // only load the svg if it's visible
        this.waitUntilVisible(this.el, '50px', () => {
            this.isVisible = true;
            this.loadIcon();
        });
    }
    disconnectedCallback() {
        if (this.io) {
            this.io.disconnect();
            this.io = undefined;
        }
    }
    waitUntilVisible(el, rootMargin, cb) {
        if (Build.isBrowser && this.lazy && typeof window !== 'undefined' && window.IntersectionObserver) {
            const io = (this.io = new window.IntersectionObserver((data) => {
                if (data[0].isIntersecting) {
                    io.disconnect();
                    this.io = undefined;
                    cb();
                }
            }, { rootMargin }));
            io.observe(el);
        }
        else {
            // browser doesn't support IntersectionObserver
            // so just fallback to always show it
            cb();
        }
    }
    loadIcon() {
        if (Build.isBrowser && this.isVisible) {
            const url = getUrl(this);
            if (url) {
                if (ioniconContent.has(url)) {
                    // sync if it's already loaded
                    this.svgContent = ioniconContent.get(url);
                }
                else {
                    // async if it hasn't been loaded
                    getSvgContent(url, this.sanitize).then(() => (this.svgContent = ioniconContent.get(url)));
                }
            }
        }
        this.iconName = getName(this.name, this.icon, this.mode, this.ios, this.md);
    }
    render() {
        const { iconName, el, inheritedAttributes } = this;
        const mode = this.mode || 'md';
        const flipRtl = this.flipRtl ||
            (iconName && (iconName.indexOf('arrow') > -1 || iconName.indexOf('chevron') > -1) && this.flipRtl !== false);
        return (h(Host, Object.assign({ role: "img", class: Object.assign(Object.assign({ [mode]: true }, createColorClasses(this.color)), { [`icon-${this.size}`]: !!this.size, 'flip-rtl': !!flipRtl && isRTL(el) }) }, inheritedAttributes), Build.isBrowser && this.svgContent ? (h("div", { class: "icon-inner", innerHTML: this.svgContent })) : (h("div", { class: "icon-inner" }))));
    }
};
__decorate([
    Element()
], Icon.prototype, "el", void 0);
__decorate([
    State()
], Icon.prototype, "svgContent", void 0);
__decorate([
    State()
], Icon.prototype, "isVisible", void 0);
__decorate([
    Prop({ mutable: true })
], Icon.prototype, "mode", void 0);
__decorate([
    Prop()
], Icon.prototype, "color", void 0);
__decorate([
    Prop()
], Icon.prototype, "ios", void 0);
__decorate([
    Prop()
], Icon.prototype, "md", void 0);
__decorate([
    Prop()
], Icon.prototype, "flipRtl", void 0);
__decorate([
    Prop({ reflect: true })
], Icon.prototype, "name", void 0);
__decorate([
    Prop()
], Icon.prototype, "src", void 0);
__decorate([
    Prop()
], Icon.prototype, "icon", void 0);
__decorate([
    Prop()
], Icon.prototype, "size", void 0);
__decorate([
    Prop()
], Icon.prototype, "lazy", void 0);
__decorate([
    Prop()
], Icon.prototype, "sanitize", void 0);
__decorate([
    Watch('name'),
    Watch('src'),
    Watch('icon'),
    Watch('ios'),
    Watch('md')
], Icon.prototype, "loadIcon", null);
Icon = __decorate([
    Component({
        tag: 'ion-icon',
        assetsDirs: ['svg'],
        styleUrl: 'icon.css',
        shadow: true,
    })
], Icon);
export { Icon };
const getIonMode = () => (Build.isBrowser && typeof document !== 'undefined' && document.documentElement.getAttribute('mode')) || 'md';
const createColorClasses = (color) => {
    return color
        ? {
            'ion-color': true,
            [`ion-color-${color}`]: true,
        }
        : null;
};
