namespace OmiLAXR.Types
{
    public enum UiElementTypes
    {
        Custom = -1,
        // --- Basic Input Controls ---
        TextField,
        TextArea,
        PasswordField,
        SearchField,
        Button,
        IconButton,
        ToggleButton,
        CheckBox,
        RadioButton,
        Switch,
        Dropdown,
        ComboBox,
        DatePicker,
        TimePicker,
        DateTimePicker,
        ColorPicker,
        FilePicker,
        Slider,
        Stepper,
        Chip,
        Scrollbar,

        // --- Containers and Layout ---
        Panel,
        Card,
        Accordion,
        Tab,
        Modal,
        Drawer,
        Sidebar,
        Toolbar,
        Grid,
        List,
        Stack,
        SplitPane,
        Section,

        // --- Navigation ---
        NavigationBar,
        Breadcrumb,
        Pagination,
        StepperNavigation,
        CommandPalette,

        // --- Feedback & Status ---
        Tooltip,
        Snackbar,
        Banner,
        HelperText,
        ValidationMessage,
        ProgressBar,
        ProgressCircle,
        SkeletonLoader,
        Notification,
        Badge,

        // --- Data Display ---
        Label,
        Heading,
        Icon,
        Image,
        Avatar,
        Table,
        Timeline,
        Calendar,
        Map,
        KeyValueView,

        // --- Data Visualization ---
        Chart,
        Heatmap,
        Gauge,
        TreeMap,
        NetworkGraph,

        // --- Interaction Enhancers ---
        ContextMenu,
        DragDropZone,
        ResizableElement,
        InlineEditor,
        Carousel,
        ZoomableArea,

        // --- Advanced Components ---
        RichTextEditor,
        MarkdownViewer,
        ChatInterface,
        Terminal,
        ThreeDViewer,
        DiagramCanvas,

        // --- Accessibility ---
        ScreenReaderRegion,
        FocusIndicator,
        SkipLink,
        ContrastToggle,
        FontSizeAdjuster,

        // --- System-Level ---
        NativeDialog,
        SystemNotification,
        ContextualMenu,
        HapticFeedback
    }
}