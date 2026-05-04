namespace MultiShop.RapidApiWebUI.Models
{
    public class ECommerceViewModel
    {

        public class Rootobject
        {
            public string status { get; set; }
            public string request_id { get; set; }
            public Data data { get; set; }
        }

        public class Data
        {
            public Product[] products { get; set; }
            public object[] sponsored_products { get; set; }
            public Filter[] filters { get; set; }
        }

        public class Product
        {
            public string product_id { get; set; }
            public string product_title { get; set; }
            public string product_description { get; set; }
            public string[] product_photos { get; set; }
            public Product_Videos[] product_videos { get; set; }
            public Product_Attributes product_attributes { get; set; }
            public float? product_rating { get; set; }
            public string product_page_url { get; set; }
            public int? product_num_reviews { get; set; }
            public int? product_num_offers { get; set; }
            public string[] typical_price_range { get; set; }
            public Current_Product_Variant_Properties current_product_variant_properties { get; set; }
            public Product_Variants product_variants { get; set; }
            public Reviews_Insights reviews_insights { get; set; }
            public Offer offer { get; set; }
        }

        public class Product_Attributes
        {
            public string TopUseCases { get; set; }
            public string TypicalUsers { get; set; }
            public string ConnectivityType { get; set; }
            public string Interface { get; set; }
            public string Compatibility { get; set; }
            public string WirelessTechnology { get; set; }
            public string USBReceiver { get; set; }
            public string VerticalDesign { get; set; }
            public string NumberofButtons { get; set; }
            public string ProgrammableButtons { get; set; }
            public string FunctionalityHighlights { get; set; }
            public string BatteryLife { get; set; }
            public string ChargingType { get; set; }
            public string TrackingMethod { get; set; }
            public string SensorType { get; set; }
            public string PollingRate { get; set; }
            public string Switches { get; set; }
            public string FeetMaterial { get; set; }
            public string MouseType { get; set; }
            public string Subtype { get; set; }
            public string Weight { get; set; }
            public string Width { get; set; }
            public string Depth { get; set; }
            public string Height { get; set; }
            public string Sensitivity { get; set; }
            public string Acceleration { get; set; }
            public string TrackingSpeed { get; set; }
            public string ProfileStorage { get; set; }
            public string Brand { get; set; }
            public string Ergonomic { get; set; }
            public string Color { get; set; }
            public string Bluetooth { get; set; }
            public string Type { get; set; }
            public string Sensor { get; set; }
            public string MultiDeviceConnectivity { get; set; }
            public string SupportedDevices { get; set; }
            public string Range { get; set; }
            public string ScrollType { get; set; }
            public string GestureSupport { get; set; }
            public string TrackingPrecision { get; set; }
            public string TrackingSurfaceType { get; set; }
            public string PowerSource { get; set; }
            public string BatteryType { get; set; }
            public string PowerIndicator { get; set; }
            public string BuildQuality { get; set; }
            public string ErgonomicSize { get; set; }
            public string Support { get; set; }
            public string WristRest { get; set; }
            public string ErgonomicHighlights { get; set; }
            public string HandOrientation { get; set; }
            public string Shape { get; set; }
            public string DesignHighlights { get; set; }
            public string GripType { get; set; }
            public string Lighting { get; set; }
            public string Material { get; set; }
            public string Finish { get; set; }
            public string MultiDevicePairing { get; set; }
            public string SilentClicks { get; set; }
            public string AdjustableWeight { get; set; }
            public string SkillLevel { get; set; }
            public string AgeRange { get; set; }
            public string LiftOffDistance { get; set; }
            public string DustResistance { get; set; }
            public string FrameRate { get; set; }
            public string VideoResolution { get; set; }
            public string ImageStabilizing { get; set; }
            public string RecordingMode { get; set; }
            public string SensorResolution { get; set; }
            public string PhotoResolution { get; set; }
            public string Resolution { get; set; }
            public string HDFormat { get; set; }
            public string OpticalZoom { get; set; }
            public string InterchangeableLens { get; set; }
            public string WirelessConnectivityType { get; set; }
            public string WiredConnectivityType { get; set; }
            public string Dimensions { get; set; }
            public string CameraWeight { get; set; }
            public string Handheld { get; set; }
            public string CameraDepth { get; set; }
            public string CameraHeight { get; set; }
            public string CameraWidth { get; set; }
            public string CameraIncluded { get; set; }
            public string CameraMountType { get; set; }
            public string CameraType { get; set; }
            public string FaceDetection { get; set; }
            public string ControlMethod { get; set; }
            public string FocusAdjustment { get; set; }
            public string UsageLocation { get; set; }
            public string NFC { get; set; }
            public string Streaming { get; set; }
            public string IncludedAccessories { get; set; }
            public string BuiltinLight { get; set; }
            public string Microphone { get; set; }
            public string MicrophoneMode { get; set; }
            public string MicrophoneFeatures { get; set; }
            public string NumberofChannels { get; set; }
            public string HighlightedFeatures { get; set; }
            public string Bundle { get; set; }
            public string CameraNotIncluded { get; set; }
            public string IndoorUse { get; set; }
            public string CableLength { get; set; }
            public string OpticalSensorSize { get; set; }
            public string OpticalSensor { get; set; }
            public string VideoFormat { get; set; }
            public string WhiteBalanceExposureMode { get; set; }
            public string WirelessInterface { get; set; }
            public string Mini { get; set; }
            public string LensElements { get; set; }
            public string MinimumFocusDistance { get; set; }
            public string FlashType { get; set; }
            public string _360DegreeCapability { get; set; }
            public string OperatingDistance { get; set; }
            public string EPEATRating { get; set; }
        }

        public class Current_Product_Variant_Properties
        {
            public string Color { get; set; }
            public string Model { get; set; }
        }

        public class Product_Variants
        {
            public Color[] Color { get; set; }
            public Model[] Model { get; set; }
        }

        public class Color
        {
            public string name { get; set; }
            public string thumbnail { get; set; }
            public string product_id { get; set; }
        }

        public class Model
        {
            public string name { get; set; }
            public string product_id { get; set; }
        }

        public class Reviews_Insights
        {
            public ErgonomicsWeightComfort ErgonomicsWeightComfort { get; set; }
            public ButtonsButtonSideButtons ButtonsButtonSideButtons { get; set; }
            public Weight Weight { get; set; }
            public ConnectivityWirelessWired ConnectivityWirelessWired { get; set; }
            public DesignColorScrollWheel DesignColorScrollWheel { get; set; }
            public Performance Performance { get; set; }
            public DPIMaximumRange DPIMaximumRange { get; set; }
            public OtherHighlights Otherhighlights { get; set; }
            public ErgonomicsShapeGripSize ErgonomicsShapeGripSize { get; set; }
            public ErgonomicsSizeComfort ErgonomicsSizeComfort { get; set; }
            public Connectivity Connectivity { get; set; }
            public ButtonsSideButtonsNumber ButtonsSideButtonsNumber { get; set; }
            public DesignScrollWheelShell DesignScrollWheelShell { get; set; }
            public Software Software { get; set; }
            public DPI DPI { get; set; }
            public ButtonsNumberButton ButtonsNumberButton { get; set; }
            public DesignCableColorBody DesignCableColorBody { get; set; }
            public SoftwareHDRResolution SoftwareHDRResolution { get; set; }
            public DesignSizeMountType DesignSizeMountType { get; set; }
            public Resolution Resolution { get; set; }
            public Microphone Microphone { get; set; }
            public MountTypeTripodClip MountTypeTripodClip { get; set; }
            public FieldOfView FieldofView { get; set; }
            public Video Video { get; set; }
            public ButtonsNumberScrollWheel ButtonsNumberScrollWheel { get; set; }
            public DesignLightingBodyColor DesignLightingBodyColor { get; set; }
            public ErgonomicsGripSizeShape ErgonomicsGripSizeShape { get; set; }
            public DPIRangeMaximum DPIRangeMaximum { get; set; }
            public ErgonomicsGripDesign ErgonomicsGripDesign { get; set; }
            public Buttons Buttons { get; set; }
            public ConnectivityWiredWireless ConnectivityWiredWireless { get; set; }
            public DesignCableBodyColor DesignCableBodyColor { get; set; }
            public SoftwareResolutionZoom SoftwareResolutionZoom { get; set; }
            public DesignMountTypeSize DesignMountTypeSize { get; set; }
            public MountTypeClipTripod MountTypeClipTripod { get; set; }
        }

        public class ErgonomicsWeightComfort
        {
            public int Comfortabletouse { get; set; }
            public int Qualitybuild { get; set; }
            public int Attractive { get; set; }
            public int Quiet { get; set; }
        }

        public class ButtonsButtonSideButtons
        {
        }

        public class Weight
        {
        }

        public class ConnectivityWirelessWired
        {
            public int Longbatterylife { get; set; }
            public int Accuratetracking { get; set; }
        }

        public class DesignColorScrollWheel
        {
        }

        public class Performance
        {
            public int Accuratetracking { get; set; }
        }

        public class DPIMaximumRange
        {
        }

        public class OtherHighlights
        {
        }

        public class ErgonomicsShapeGripSize
        {
            public int Comfortabletouse { get; set; }
            public int Attractive { get; set; }
            public int Qualitybuild { get; set; }
            public int Easytoclean { get; set; }
            public int Easytosetup { get; set; }
        }

        public class ErgonomicsSizeComfort
        {
            public int Comfortabletouse { get; set; }
            public int Attractive { get; set; }
        }

        public class Connectivity
        {
            public int Longbatterylife { get; set; }
            public int Accuratetracking { get; set; }
        }

        public class ButtonsSideButtonsNumber
        {
            public int Quiet { get; set; }
            public int Easytouse { get; set; }
        }

        public class DesignScrollWheelShell
        {
        }

        public class Software
        {
            public int Easytouse { get; set; }
            public int Qualitybuild { get; set; }
            public int Easytosetup { get; set; }
        }

        public class DPI
        {
        }

        public class ButtonsNumberButton
        {
            public int Easytouse { get; set; }
        }

        public class DesignCableColorBody
        {
        }

        public class SoftwareHDRResolution
        {
            public int Goodcompatibility { get; set; }
            public int Easytouse { get; set; }
        }

        public class DesignSizeMountType
        {
            public int Compact { get; set; }
            public int Mountssecurely { get; set; }
            public int Attractive { get; set; }
            public int Longcord { get; set; }
            public int Qualitybuild { get; set; }
        }

        public class Resolution
        {
        }

        public class Microphone
        {
            public int Qualityaudio { get; set; }
        }

        public class MountTypeTripodClip
        {
        }

        public class FieldOfView
        {
        }

        public class Video
        {
            public int Qualitypicture { get; set; }
        }

        public class ButtonsNumberScrollWheel
        {
        }

        public class DesignLightingBodyColor
        {
        }

        public class ErgonomicsGripSizeShape
        {
            public int Comfortabletouse { get; set; }
            public int Qualitybuild { get; set; }
            public int Attractive { get; set; }
            public int Easytosetup { get; set; }
        }

        public class DPIRangeMaximum
        {
        }

        public class ErgonomicsGripDesign
        {
            public int Comfortabletouse { get; set; }
            public int Qualitybuild { get; set; }
            public int Attractive { get; set; }
        }

        public class Buttons
        {
            public int Easytouse { get; set; }
        }

        public class ConnectivityWiredWireless
        {
            public int Longbatterylife { get; set; }
        }

        public class DesignCableBodyColor
        {
        }

        public class SoftwareResolutionZoom
        {
            public int Goodcompatibility { get; set; }
            public int Easytosetup { get; set; }
            public int Easytouse { get; set; }
        }

        public class DesignMountTypeSize
        {
            public int Attractive { get; set; }
            public int Qualitybuild { get; set; }
        }

        public class MountTypeClipTripod
        {
        }

        public class Offer
        {
            public string offer_id { get; set; }
            public string offer_title { get; set; }
            public string offer_page_url { get; set; }
            public string price { get; set; }
            public string original_price { get; set; }
            public bool on_sale { get; set; }
            public string shipping { get; set; }
            public string returns { get; set; }
            public string offer_badge { get; set; }
            public string product_condition { get; set; }
            public string store_name { get; set; }
            public string store_rating { get; set; }
            public int store_review_count { get; set; }
            public string store_reviews_page_url { get; set; }
            public string store_favicon { get; set; }
            public string payment_methods { get; set; }
            public string special_offer { get; set; }
            public string special_offer_expiration { get; set; }
            public string coupon_discount { get; set; }
            public string coupon_expiration { get; set; }
            public string percent_off { get; set; }
        }

        public class Product_Videos
        {
            public string title { get; set; }
            public string url { get; set; }
            public string source { get; set; }
            public string publisher { get; set; }
            public string thumbnail { get; set; }
            public int duration_ms { get; set; }
            public string preview_url { get; set; }
        }

        public class Filter
        {
            public string title { get; set; }
            public bool multivalue { get; set; }
            public Value[] values { get; set; }
        }

        public class Value
        {
            public string title { get; set; }
            public string q { get; set; }
            public string shoprs { get; set; }
        }

    }
}
