using System;
using System.Configuration;
using Facebook; //Facebook C# SDK için Namespace
namespace FacebookLogin.Models
{
    public class FacebookManager
    {
        #region Verbs
        /// <summary>
        /// Facebook API kullanımı için gerekli App Id değeri
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        /// Facebook API kullanımı için gerekli App Secret değeri
        /// </summary>
        public string AppSecret { get; set; }
        /// <summary>
        /// Yetkilendirme işleminden sonra dönüş yapılacak adres
        /// Ör. localhost/Facebook/CallBack
        /// </summary>
        public string CallBackURL { get; set; }
        /// <summary>
        /// Facebook için izinler. Her bir izin için virgül ile ayrılır.
        /// Ör. user_about_me,email,user_likes,user_friends,user_birthday
        /// Detay: https://developers.facebook.com/docs/facebook-login/permissions/v2.0
        /// </summary>
        public string Scope { get; set; }
        FacebookClient fb = new FacebookClient();
        #endregion
        #region Constructors
        public FacebookManager(string appId, string appSecret, string callBackUrl, string scope)
        {
            AppId = appId;
            AppSecret = appSecret;
            CallBackURL = callBackUrl;
            Scope = scope;
        }
        /// <summary>
        /// Değerleri web.config, app.config gibi dosyalardan almak için kullanabilirsiniz.
        /// </summary>
        public FacebookManager()
        {
            AppId = ConfigurationManager.AppSettings["Facebook.AppId"];
            AppSecret = ConfigurationManager.AppSettings["Facebook.AppSecret"];
            CallBackURL = ConfigurationManager.AppSettings["Facebook.CallBackURL"];
            Scope = ConfigurationManager.AppSettings["Facebook.Scope"];
        }
        #endregion
        #region Methods
        /// <summary>
        /// Yetkilendirme için gerekli değerlerimiz ile Facebook.com'a yönleneceğimiz adresi oluşturur
        /// </summary>
        /// <returns></returns>
        public string GetLoginUrl()
        {
                        return fb.GetLoginUrl(
                                new
                                {
                                    client_id = AppId,
                                    client_secret = AppSecret,
                                    redirect_uri = CallBackURL,
                                    response_type = "code",
                                    scope = Scope,

                                }).ToString();
            //return fb.GetLoginUrl(new { client_id = AppId, client_secret = AppSecret, redirect_uri = CallBackURL, scope = Scope, response_type = "code" }).ToString();
        }
        /// <summary>
        /// Facebook'un OAuth işlemleri yapabilmemiz için verdiği erişim kodudur.
        /// </summary>
        public dynamic GetAccessToken(string _code)
        {
            dynamic result = fb.Post("oauth/access_token",
                                          new
                                          {
                                              client_id = AppId,
                                              client_secret = AppSecret,
                                              redirect_uri = CallBackURL,
                                              code = _code
                                          });
            return result.access_token;

        }
        /// <summary>
        /// Kullanıcı bilgilerini çektiğimiz methodumuz
        /// </summary>
        /// <returns></returns>
        public dynamic GetUserInfo(string accessToken)
        {
            // Kullanıcı bilgilerini çektiğimiz metod
            var client = new FacebookClient(accessToken);
            // me yerine kullanıcının facebook id'sini yazabilirsiniz, bir diğer methodumuzda arkadaş listemize erişeceğiz o kısımdaki id'ler işinizi görür
            // me kullanıcısı uygulamaya izin verip login olan kişinin bilgilerini temsil eder
            dynamic me = client.Get("/me");
            me.picture = string.Format("https://graph.facebook.com/{0}/picture?type=large", me.id);
            return me;
        }
        #endregion
    }
}