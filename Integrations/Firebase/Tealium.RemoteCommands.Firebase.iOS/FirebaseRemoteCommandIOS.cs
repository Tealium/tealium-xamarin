﻿using System;
using System.Collections.Generic;

using Tealium;
using Tealium.Platform.iOS;

using Firebase.Analytics;
using Firebase.Core;

namespace Tealium.RemoteCommands.Firebase.iOS
{
    public class FirebaseRemoteCommandIOS : FirebaseRemoteCommand, IRemoteCommand
    {
        //private static AnalyticsConfiguration fireAnalyticsConfig;
        private static Configuration fireConfig = Configuration.SharedInstance;

        private RemoteCommandTypeWrapper type;

        public override string Path => type.Path;

        public override string Url => type.Url;

        static FirebaseRemoteCommandIOS()
        {
            eventsMap.Add("event_add_payment_info", EventNamesConstants.AddPaymentInfo);
            eventsMap.Add("event_add_shipping_info", EventNamesConstants.AddShippingInfo);
            eventsMap.Add("add_to_cart", EventNamesConstants.AddToCart);
            eventsMap.Add("add_to_wishlist", EventNamesConstants.AddToWishlist);
            eventsMap.Add("app_open", EventNamesConstants.AppOpen);
            eventsMap.Add("event_begin_checkout", EventNamesConstants.BeginCheckout);
            eventsMap.Add("event_campaign_details", EventNamesConstants.CampaignDetails);
            eventsMap.Add("event_checkout_progress", EventNamesConstants.CheckoutProgress);
            eventsMap.Add("event_earn_virtual_currency", EventNamesConstants.EarnVirtualCurrency);
            eventsMap.Add("event_ecommerce_purchase", EventNamesConstants.EcommercePurchase);
            eventsMap.Add("event_generate_lead", EventNamesConstants.GenerateLead);
            eventsMap.Add("event_join_group", EventNamesConstants.JoinGroup);
            eventsMap.Add("event_level_end", EventNamesConstants.LevelEnd);
            eventsMap.Add("event_level_start", EventNamesConstants.LevelStart);
            eventsMap.Add("event_level_up", EventNamesConstants.LevelUp);
            eventsMap.Add("event_login", EventNamesConstants.Login);
            eventsMap.Add("event_post_score", EventNamesConstants.PostScore);
            eventsMap.Add("event_present_offer", EventNamesConstants.PresentOffer);
            eventsMap.Add("event_ecommerce_purchase", EventNamesConstants.EcommercePurchase);
            eventsMap.Add("event_purchase", EventNamesConstants.Purchase);
            eventsMap.Add("event_purchase_refund", EventNamesConstants.PurchaseRefund);
            eventsMap.Add("event_refund", EventNamesConstants.Refund);
            eventsMap.Add("event_remove_cart", EventNamesConstants.RemoveFromCart);
            eventsMap.Add("event_search", EventNamesConstants.Search);
            eventsMap.Add("event_select_content", EventNamesConstants.SelectContent);
            eventsMap.Add("event_select_item", EventNamesConstants.SelectItem);
            eventsMap.Add("event_select_promotion", EventNamesConstants.SelectPromotion);
            eventsMap.Add("event_set_checkout_option", EventNamesConstants.SetCheckoutOption);
            eventsMap.Add("event_share", EventNamesConstants.Share);
            eventsMap.Add("event_signup", EventNamesConstants.SignUp);
            eventsMap.Add("event_spend_virtual_currency", EventNamesConstants.SpendVirtualCurrency);
            eventsMap.Add("event_tutorial_begin", EventNamesConstants.TutorialBegin);
            eventsMap.Add("event_tutorial_complete", EventNamesConstants.TutorialComplete);
            eventsMap.Add("event_unlock_achievement", EventNamesConstants.UnlockAchievement);
            eventsMap.Add("event_view_item", EventNamesConstants.ViewItem);
            eventsMap.Add("event_view_item_list", EventNamesConstants.ViewItemList);
            eventsMap.Add("event_view_search_results", EventNamesConstants.ViewSearchResults);


            parameters.Add("param_achievement_id", ParameterNamesConstants.AchievementId);
            parameters.Add("param_ad_network_click_id", ParameterNamesConstants.AdNetworkClickId);
            parameters.Add("param_affiliation", ParameterNamesConstants.Affiliation);
            parameters.Add("param_cp1", ParameterNamesConstants.Cp1);
            parameters.Add("param_campaign", ParameterNamesConstants.Campaign);
            parameters.Add("param_character", ParameterNamesConstants.Character);
            parameters.Add("param_checkout_option", ParameterNamesConstants.CheckoutOption);
            parameters.Add("param_checkout_step", ParameterNamesConstants.CheckoutStep);
            parameters.Add("param_content", ParameterNamesConstants.Content);
            parameters.Add("param_content_type", ParameterNamesConstants.ContentType);
            parameters.Add("param_coupon", ParameterNamesConstants.Coupon);
            parameters.Add("param_creative_name", ParameterNamesConstants.CreativeName);
            parameters.Add("param_creative_slot", ParameterNamesConstants.CreativeSlot);
            parameters.Add("param_currency", ParameterNamesConstants.Currency);
            parameters.Add("param_destination", ParameterNamesConstants.Destination);
            parameters.Add("param_discount", ParameterNamesConstants.Discount);
            parameters.Add("param_end_date", ParameterNamesConstants.EndDate);
            parameters.Add("param_extend_session", ParameterNamesConstants.ExtendSession);
            parameters.Add("param_flight_number", ParameterNamesConstants.FlightNumber);
            parameters.Add("param_group_id", ParameterNamesConstants.GroupId);
            parameters.Add("param_index", ParameterNamesConstants.Index);
            parameters.Add("param_item_brand", ParameterNamesConstants.ItemBrand);
            parameters.Add("param_item_category", ParameterNamesConstants.ItemCategory);
            parameters.Add("param_item_id", ParameterNamesConstants.ItemId);
            parameters.Add("param_item_list", ParameterNamesConstants.ItemList);
            parameters.Add("param_item_list_id", ParameterNamesConstants.ItemListId);
            parameters.Add("param_item_list_name", ParameterNamesConstants.ItemListName);
            parameters.Add("param_item_location_id", ParameterNamesConstants.ItemLocationId);
            parameters.Add("param_item_name", ParameterNamesConstants.ItemName);
            parameters.Add("param_item_variant", ParameterNamesConstants.ItemVariant);
            parameters.Add("param_level", ParameterNamesConstants.Level);
            parameters.Add("param_level_name", ParameterNamesConstants.LevelName);
            parameters.Add("param_location", ParameterNamesConstants.Location);
            parameters.Add("param_location_id", ParameterNamesConstants.LocationId);
            parameters.Add("param_medium", ParameterNamesConstants.Medium);
            parameters.Add("param_method", ParameterNamesConstants.Method);
            parameters.Add("param_number_nights", ParameterNamesConstants.NumberOfNights);
            parameters.Add("param_number_pax", ParameterNamesConstants.NumberOfPassengers);
            parameters.Add("param_number_rooms", ParameterNamesConstants.NumberOfRooms);
            parameters.Add("param_origin", ParameterNamesConstants.Origin);
            parameters.Add("param_payment_type", ParameterNamesConstants.PaymentType);
            parameters.Add("param_price", ParameterNamesConstants.Price);
            parameters.Add("param_promotion_id", ParameterNamesConstants.PromotionId);
            parameters.Add("param_promotion_name", ParameterNamesConstants.PromotionName);
            parameters.Add("param_quantity", ParameterNamesConstants.Quantity);
            parameters.Add("param_score", ParameterNamesConstants.Score);
            parameters.Add("param_search_term", ParameterNamesConstants.SearchTerm);
            parameters.Add("param_shipping", ParameterNamesConstants.Shipping);
            parameters.Add("param_shipping_tier", ParameterNamesConstants.ShippingTier);
            parameters.Add("param_signup_method", ParameterNamesConstants.SignUpMethod);
            parameters.Add("param_source", ParameterNamesConstants.Source);
            parameters.Add("param_start_date", ParameterNamesConstants.StartDate);
            parameters.Add("param_success", ParameterNamesConstants.Success);
            parameters.Add("param_tax", ParameterNamesConstants.Tax);
            parameters.Add("param_term", ParameterNamesConstants.Term);
            parameters.Add("param_transaction_id", ParameterNamesConstants.TransactionId);
            parameters.Add("param_travel_class", ParameterNamesConstants.TravelClass);
            parameters.Add("param_value", ParameterNamesConstants.Value);
            parameters.Add("param_virtual_currency_name", ParameterNamesConstants.VirtualCurrencyName);
            parameters.Add("param_user_signup_method", UserPropertyNamesConstants.SignUpMethod);

        }


        public FirebaseRemoteCommandIOS(RemoteCommandTypeWrapper type) : base()
        {
            this.type = type;
        }

        protected override void Configure(int? sessionTimeoutDuration, int? minSessionDuration, bool? analyticsEnabled)
        {
            if (sessionTimeoutDuration.HasValue)
            {
                Analytics.SetSessionTimeoutInterval(sessionTimeoutDuration.Value);
            }
            if (analyticsEnabled.HasValue)
            {
                Analytics.SetAnalyticsCollectionEnabled(analyticsEnabled.Value);
            }
            
            if (App.DefaultInstance == null)
            {
                //Can't call configure more than once... errors.
                App.Configure();
            }
        }

        protected override void LogEvent(string eventName, Dictionary<String, String> eventParams)
        {
            //if(IsNullOrNullString(eventName)) { return; }
            Dictionary<object, object> dict = new Dictionary<object, object>();
            foreach (var key in eventParams.Keys)
            {
                dict.Add(mapParams(key), eventParams.GetValueOrDefault(key));
            }
            Analytics.LogEvent(eventName, dict);
        }

        protected override void SetScreenName(string screenName, string screenClass)
        {
            //if(IsNullOrNullString(screenName) || IsNullOrNullString(screenClass)) { return; }
            //Analytics.SetScreenNameAndClass(screenName, screenClass);
            Analytics.LogEvent(EventNamesConstants.ScreenView, new Dictionary<object, object>
            {
                { ParameterNamesConstants.ScreenName, screenName },
                { ParameterNamesConstants.ScreenClass, screenClass }
            });
        }

        protected override void SetUserProperty(string propertyName, string propertyValue)
        {
            //if (IsNullOrNullString(propertyName) || IsNullOrNullString(propertyValue)) { return; }
            Analytics.SetUserProperty(propertyName, propertyValue);
        }

        protected override void SetUserId(string userId)
        {
            Analytics.SetUserId(userId);
        }
    }
}