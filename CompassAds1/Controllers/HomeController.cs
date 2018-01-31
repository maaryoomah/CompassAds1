using CompassAds1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

//APPNhOpLs2EVTJQj2A92c++gpKNajmE3ieq1q4KtE4EoWuu5gV3wUkAwiHmlKKF+dQ==
//AJXOnsmHe2/poF+t5xlDEDHgHSt/hnOHw41wAjQez9fNGRdrO2cAKUUEfAb1QYgWlg==
namespace CompassAds1.Controllers
{
    public class HomeController : Controller
    {
        AompassAdsEntities4 db = new AompassAdsEntities4();
        ApplicationDbContext Adb = new ApplicationDbContext();

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";


            return View();
        }



        public ActionResult Index()
        {
            //  ViewBag.Message = "Your application description page.";
            ViewBag.countries = new SelectList(db.CountryCallingCodes, "id", "Country");


            return View();
        }

        public String Fail()
        {
            return "Fail";
        }



        //---------------------------------------------------------

        [AllowAnonymous]
        [HttpPost]
        public JsonResult Emailalready(String id)
        {


            if (id != "")
            {
                var email = Adb.Users.Any(u => u.Email == id);
                // var idnum = _user.IdNumber.ToString();
                // return Json(!Adb.Users.Any(u => u.IdNumber == id), JsonRequestBehavior.AllowGet);
                if (email)
                {
                    return Json(1);
                }
                return Json(0);


            }

            return Json(2);


        }



        //-------------------------------------------------------------------------



        [AllowAnonymous]
        [HttpPost]
        public String SelectCode(String id) // alaways using id parameter as aname for any parameter becouse it is the default parameter name on routconfig file
        {


            if (id != "")
            {
                var code = db.CountryCallingCodes.Single(r => r.Id.ToString() == id);

                // var idnum = _user.IdNumber.ToString();
                // return Json(!Adb.Users.Any(u => u.IdNumber == id), JsonRequestBehavior.AllowGet);
                return code.callingcode;
            }

            return "";


        }


        public ActionResult Gallery()
        {
            List<Gallary> all = new List<Gallary>();

            // Here MyDatabaseEntities is our datacontext

            all = db.Gallary.ToList();

            return View(all);
        }


        public ActionResult Upload()
        {
            return View();
        }

      //  [HttpPost]
    /*    public string Upload(HttpPostedFileBase file)
        {
            // Apply Validation Here


            /*  if (IG.File.ContentLength > (2 * 1024 * 1024))
              {
                  ModelState.AddModelError("CustomError", "File size must be less than 2 MB");
                  //return View();
                  return "1";
              }
              if (!(IG.File.ContentType == "image/jpg" || IG.File.ContentType == "image/gif"))
              {
                  ModelState.AddModelError("CustomError", "File type allowed : jpeg and gif");
                  // return View();
                  return "2";
              }*/




            /* IG.FileName = IG.File.FileName;
             IG.ImageSize = IG.File.ContentLength;

             byte[] data = new byte[IG.File.ContentLength];
             IG.File.InputStream.Read(data, 0, IG.File.ContentLength);

             IG.dataimg = data;

             db.Gallary.Add(IG);
             db.SaveChanges();

             return RedirectToAction("Gallery");*/
      //      return file.FileName;
      //  }

    




              

       [HttpPost]
        public string Upload(FormCollection photo) //او/// FormCollection but مع الريكويست في فايل        HttpPostedFileBase
        {
            //PhotoForSingleItem is just a class that has properties
            // Name and Alternate text.  I use strongly typed Views and Actions
            //  because I'm not a fan of using string to get the posted data from the
            //  FormCollection.  That just seems ugly and unreliable to me.

            //PhotoViewImage is just a Entityframework class that has
            // String Name, String AlternateText, Byte[] ActualImage,
            //  and String ContentType
            PhotoViewImage newImage = new PhotoViewImage();
            HttpPostedFileBase file = Request.Files["FileUpload"];


            //Here's where the ContentType column comes in handy.  By saving
            //  this to the database, it makes it infinitely easier to get it back
            //  later when trying to show the image.
            // newImage.ContentType = file.ContentType;

            Int32 length = file.ContentLength;
            //This may seem odd, but the fun part is that if
            //  I didn't have a temp image to read into, I would
            //  get memory issues for some reason.  Something to do
            //  with reading straight into the object's ActualImage property.
           byte[] tempImage = new byte[length];
             file.InputStream.Read(tempImage, 0, length);
            //newImage.ActualImage = tempImage;
            Gallary G = new Gallary();

           String id = Guid.NewGuid().ToString();
            var isidexsist1 = db.Gallary.Any(c => c.id == id);     // return boolean


            while (isidexsist1)
            {

                id = Guid.NewGuid().ToString();
                 isidexsist1 = db.Gallary.Any(c => c.id == id);
            }
            G.id = id;

            G.dataimg = tempImage;
           // G.ImageSize = length;
            G.ImageName = file.FileName;


              db.Gallary.Add(G);
              db.SaveChanges();




            //This part is completely optional.  You could redirect on success
            // or handle errors ect.  Just wanted to keep this simple for the example.
            return tempImage[0]+ tempImage[1]+ tempImage[2]+ tempImage[3]+ tempImage[4] + "";





            //----------------------------------------------------------------------------------------------------------


            String THTest()
            {


                /*   Guid ID1;
                   ID1 = Guid.NewGuid();
                   var isidexsist1 = db.CountryCallingCodes.Any(c => c.Id == ID1);     // return boolean


                    while (isidexsist1)
                    {

                       ID1 = Guid.NewGuid();
                        isidexsist1 = db.CountryCallingCodes.Any(c => c.Id == ID1);
                   }

                   CountryCallingCodes ccc = new CountryCallingCodes();
                   ccc.Id = ID1;
                   ccc.country = "Afghanistan";
                   ccc.callingcode = "+93";

                    db.CountryCallingCodes.Add(ccc);


                  //--------------------------------------------------

                   ID1 = Guid.NewGuid();
                    isidexsist1 = db.CountryCallingCodes.Any(c => c.Id == ID1);     // return boolean


                   while (isidexsist1)
                   {

                       ID1 = Guid.NewGuid();
                       isidexsist1 = db.CountryCallingCodes.Any(c => c.Id == ID1);
                   }

                    ccc = new CountryCallingCodes();
                   ccc.Id = ID1;
                   ccc.country = " Albania";
                   ccc.callingcode = "+355";

                   db.CountryCallingCodes.Add(ccc);


                   //-----------------------------------------------------------------------------

                   ID1 = Guid.NewGuid();
                    isidexsist1 = db.CountryCallingCodes.Any(c => c.Id == ID1);     // return boolean


                   while (isidexsist1)
                   {

                       ID1 = Guid.NewGuid();
                       isidexsist1 = db.CountryCallingCodes.Any(c => c.Id == ID1);
                   }

                    ccc = new CountryCallingCodes();
                   ccc.Id = ID1;
                   ccc.country = "Algeria";
                   ccc.callingcode = "+213";

                   db.CountryCallingCodes.Add(ccc);


                   //-----------------------------------------------------------------------

                   ID1 = Guid.NewGuid();
                    isidexsist1 = db.CountryCallingCodes.Any(c => c.Id == ID1);     // return boolean


                   while (isidexsist1)
                   {

                       ID1 = Guid.NewGuid();
                       isidexsist1 = db.CountryCallingCodes.Any(c => c.Id == ID1);
                   }

                    ccc = new CountryCallingCodes();
                   ccc.Id = ID1;
                   ccc.country = "Andorra";
                   ccc.callingcode = "+376";

                   db.CountryCallingCodes.Add(ccc);


                   //-----------------------------------------------------------------

                   ID1 = Guid.NewGuid();
                    isidexsist1 = db.CountryCallingCodes.Any(c => c.Id == ID1);     // return boolean


                   while (isidexsist1)
                   {

                       ID1 = Guid.NewGuid();
                       isidexsist1 = db.CountryCallingCodes.Any(c => c.Id == ID1);
                   }

                   ccc = new CountryCallingCodes();
                   ccc.Id = ID1;
                   ccc.country = " Angola";
                   ccc.callingcode = "+244";

                   db.CountryCallingCodes.Add(ccc);





                   */


                //لهون DONE

                //--------------------------------------------------------------------



                /*   Guid ID1;
                   ID1 = Guid.NewGuid();
                   var isidexsist1 = db.CountryCallingCodes.Any(c => c.Id == ID1);     // return boolean


                   while (isidexsist1)
                   {

                       ID1 = Guid.NewGuid();
                       isidexsist1 = db.CountryCallingCodes.Any(c => c.Id == ID1);
                   }

                   CountryCallingCodes ccc = new CountryCallingCodes();
                   ccc.Id = ID1;
                   ccc.country = "";
                   ccc.callingcode = "";

                   db.CountryCallingCodes.Add(ccc);




                   //---------------------------------------------------------

                   Guid ID1;
                   ID1 = Guid.NewGuid();
                   var isidexsist1 = db.CountryCallingCodes.Any(c => c.Id == ID1);     // return boolean


                   while (isidexsist1)
                   {

                       ID1 = Guid.NewGuid();
                       isidexsist1 = db.CountryCallingCodes.Any(c => c.Id == ID1);
                   }

                   CountryCallingCodes ccc = new CountryCallingCodes();
                   ccc.Id = ID1;
                   ccc.country = "Afghanistan";
                   ccc.callingcode = "+93";

                   db.CountryCallingCodes.Add(ccc);


                   //----------------------------------------------------------------------------
                   Guid ID1;
                   ID1 = Guid.NewGuid();
                   var isidexsist1 = db.CountryCallingCodes.Any(c => c.Id == ID1);     // return boolean


                   while (isidexsist1)
                   {

                       ID1 = Guid.NewGuid();
                       isidexsist1 = db.CountryCallingCodes.Any(c => c.Id == ID1);
                   }

                   CountryCallingCodes ccc = new CountryCallingCodes();
                   ccc.Id = ID1;
                   ccc.country = "Afghanistan";
                   ccc.callingcode = "+93";

                   db.CountryCallingCodes.Add(ccc);


                   //-----------------------------------------------------------------
                   Guid ID1;
                   ID1 = Guid.NewGuid();
                   var isidexsist1 = db.CountryCallingCodes.Any(c => c.Id == ID1);     // return boolean


                   while (isidexsist1)
                   {

                       ID1 = Guid.NewGuid();
                       isidexsist1 = db.CountryCallingCodes.Any(c => c.Id == ID1);
                   }

                   CountryCallingCodes ccc = new CountryCallingCodes();
                   ccc.Id = ID1;
                   ccc.country = "Afghanistan";
                   ccc.callingcode = "+93";

                   db.CountryCallingCodes.Add(ccc);


                   //------------------------------------------------------------------------------ 
                   Guid ID1;
                   ID1 = Guid.NewGuid();
                   var isidexsist1 = db.CountryCallingCodes.Any(c => c.Id == ID1);     // return boolean


                   while (isidexsist1)
                   {

                       ID1 = Guid.NewGuid();
                       isidexsist1 = db.CountryCallingCodes.Any(c => c.Id == ID1);
                   }

                   CountryCallingCodes ccc = new CountryCallingCodes();
                   ccc.Id = ID1;
                   ccc.country = "Afghanistan";
                   ccc.callingcode = "+93";

                   db.CountryCallingCodes.Add(ccc);


                   //-----------------------------------------------------------------------------------------------

                   Guid ID1;
                   ID1 = Guid.NewGuid();
                   var isidexsist1 = db.CountryCallingCodes.Any(c => c.Id == ID1);     // return boolean


                   while (isidexsist1)
                   {

                       ID1 = Guid.NewGuid();
                       isidexsist1 = db.CountryCallingCodes.Any(c => c.Id == ID1);
                   }

                   CountryCallingCodes ccc = new CountryCallingCodes();
                   ccc.Id = ID1;
                   ccc.country = "Afghanistan";
                   ccc.callingcode = "+93";

                   db.CountryCallingCodes.Add(ccc);

                   //------------------------------------------------------------------------
                   Guid ID1;
                   ID1 = Guid.NewGuid();
                   var isidexsist1 = db.CountryCallingCodes.Any(c => c.Id == ID1);     // return boolean


                   while (isidexsist1)
                   {

                       ID1 = Guid.NewGuid();
                       isidexsist1 = db.CountryCallingCodes.Any(c => c.Id == ID1);
                   }

                   CountryCallingCodes ccc = new CountryCallingCodes();
                   ccc.Id = ID1;
                   ccc.country = "Afghanistan";
                   ccc.callingcode = "+93";

                   db.CountryCallingCodes.Add(ccc);

                   //-------------------------------------------------------------------------------------------

                   Guid ID1;
                   ID1 = Guid.NewGuid();
                   var isidexsist1 = db.CountryCallingCodes.Any(c => c.Id == ID1);     // return boolean


                   while (isidexsist1)
                   {

                       ID1 = Guid.NewGuid();
                       isidexsist1 = db.CountryCallingCodes.Any(c => c.Id == ID1);
                   }

                   CountryCallingCodes ccc = new CountryCallingCodes();
                   ccc.Id = ID1;
                   ccc.country = "Afghanistan";
                   ccc.callingcode = "+93";

                   db.CountryCallingCodes.Add(ccc);

                   //-------------------------------------------------------------------------------------

                   Guid ID1;
                   ID1 = Guid.NewGuid();
                   var isidexsist1 = db.CountryCallingCodes.Any(c => c.Id == ID1);     // return boolean


                   while (isidexsist1)
                   {

                       ID1 = Guid.NewGuid();
                       isidexsist1 = db.CountryCallingCodes.Any(c => c.Id == ID1);
                   }

                   CountryCallingCodes ccc = new CountryCallingCodes();
                   ccc.Id = ID1;
                   ccc.country = "Afghanistan";
                   ccc.callingcode = "+93";

                   db.CountryCallingCodes.Add(ccc);

                   //----------------------------------------------------------------------------------------

                   Guid ID1;
                   ID1 = Guid.NewGuid();
                   var isidexsist1 = db.CountryCallingCodes.Any(c => c.Id == ID1);     // return boolean


                   while (isidexsist1)
                   {

                       ID1 = Guid.NewGuid();
                       isidexsist1 = db.CountryCallingCodes.Any(c => c.Id == ID1);
                   }

                   CountryCallingCodes ccc = new CountryCallingCodes();
                   ccc.Id = ID1;
                   ccc.country = "Afghanistan";
                   ccc.callingcode = "+93";

                   db.CountryCallingCodes.Add(ccc);

                   //----------------------------------------------------------------------------------------------

                   Guid ID1;
                   ID1 = Guid.NewGuid();
                   var isidexsist1 = db.CountryCallingCodes.Any(c => c.Id == ID1);     // return boolean


                   while (isidexsist1)
                   {

                       ID1 = Guid.NewGuid();
                       isidexsist1 = db.CountryCallingCodes.Any(c => c.Id == ID1);
                   }

                   CountryCallingCodes ccc = new CountryCallingCodes();
                   ccc.Id = ID1;
                   ccc.country = "Afghanistan";
                   ccc.callingcode = "+93";

                   db.CountryCallingCodes.Add(ccc);

                   //----------------------------------------------------------------------------------------------------------------

                   Guid ID1;
                   ID1 = Guid.NewGuid();
                   var isidexsist1 = db.CountryCallingCodes.Any(c => c.Id == ID1);     // return boolean


                   while (isidexsist1)
                   {

                       ID1 = Guid.NewGuid();
                       isidexsist1 = db.CountryCallingCodes.Any(c => c.Id == ID1);
                   }

                   CountryCallingCodes ccc = new CountryCallingCodes();
                   ccc.Id = ID1;
                   ccc.country = "Afghanistan";
                   ccc.callingcode = "+93";

                   db.CountryCallingCodes.Add(ccc);


                   //-------------------------------------------------------------------------

                   Guid ID1;
                   ID1 = Guid.NewGuid();
                   var isidexsist1 = db.CountryCallingCodes.Any(c => c.Id == ID1);     // return boolean


                   while (isidexsist1)
                   {

                       ID1 = Guid.NewGuid();
                       isidexsist1 = db.CountryCallingCodes.Any(c => c.Id == ID1);
                   }

                   CountryCallingCodes ccc = new CountryCallingCodes();
                   ccc.Id = ID1;
                   ccc.country = "Afghanistan";
                   ccc.callingcode = "+93";

                   db.CountryCallingCodes.Add(ccc);

                   //--------------------------------------------------------------------------------------

                   Guid ID1;
                   ID1 = Guid.NewGuid();
                   var isidexsist1 = db.CountryCallingCodes.Any(c => c.Id == ID1);     // return boolean


                   while (isidexsist1)
                   {

                       ID1 = Guid.NewGuid();
                       isidexsist1 = db.CountryCallingCodes.Any(c => c.Id == ID1);
                   }

                   CountryCallingCodes ccc = new CountryCallingCodes();
                   ccc.Id = ID1;
                   ccc.country = "Afghanistan";
                   ccc.callingcode = "+93";

                   db.CountryCallingCodes.Add(ccc);





























                   */



                db.SaveChanges();









                return "done";

            }

            //-------------------------------------------------------------------


            /*





            American Samoa 1 684

            Anguilla 1 264
            Antarctica (Australian bases) 6721
            Antigua and Barbuda 1 268
            Argentina 54
            Armenia 374
            Aruba 297
            Ascension 247
            Australia 61
            Austria 43
            Azerbaijan 994
            Bahamas 1 242
            Bahrain 973
            Bangladesh 880
            Barbados 1 246
            Belarus 375
            Belgium 32
            Belize 501
            Benin 229
            Bermuda 1 441
            Bhutan 975
            Bolivia 591
            Bosnia and Herzegovina 387
            Botswana 267
            Brazil 55
            British Indian Ocean Territory 246
            British Virgin Islands 1 284
            Brunei 673
            Bulgaria 359
            Burkina Faso 226
            Burundi 257
            Cambodia 855
            Cameroon 237
            Canada 1
            Cape Verde 238
            Cayman Islands 1 345
            Central African Republic 236
            Chad 235
            Chile 56
            China 86
            Colombia 57
            Comoros 269
            Congo, Democratic Republic of the 243
            Congo, Republic of the 242
            Cook Islands 682
            Costa Rica 506
            Cote d'Ivoire 225
            Croatia 385
            Cuba 53
            Curaçao 599
            Cyprus 357
            Czech Republic 420
            Denmark 45
            Djibouti 253
            Dominica 1 767
            Dominican Republic 1 809, 1 829, and 1 849
            East Timor 670
            Ecuador 593
            Egypt 20
            El Salvador 503
            Equatorial Guinea 240
            Eritrea 291
            Estonia 372
            Ethiopia 251
            Falkland Islands 500
            Faroe Islands 298
            Fiji 679
            Finland 358
            France 33
            French Guiana 594
            French Polynesia 689
            Gabon 241
            Gambia 220
            Gaza Strip 970
            Georgia (and parts of breakaway regions Abkhazia as well as South Ossetia) 995
            Germany 49
            Ghana 233
            Gibraltar 350
            Greece 30
            Greenland 299
            Grenada 1 473
            Guadeloupe 590
            Guam 1 671
            Guatemala 502
            Guinea 224
            Guinea-Bissau 245
            Guyana 592
            Haiti 509
            Honduras 504
            Hong Kong 852
            Hungary 36
            Iceland 354
            India 91
            Indonesia 62
            Iraq 964
            Iran 98
            Ireland (Eire) 353
            Israel 972
            Italy 39
            Jamaica 1 876
            Japan 81
            Jordan 962
            Kazakhstan 7
            Kenya 254
            Kiribati 686
            Kosovo 383
            Kuwait 965
            Kyrgyzstan 996
            Laos 856
            Latvia 371
            Lebanon 961
            Lesotho 266
            Liberia 231
            Libya 218
            Liechtenstein 423
            Lithuania 370
            Luxembourg 352
            Macau 853
            Macedonia, Republic of 389
            Madagascar 261
            Malawi 265
            Malaysia 60
            Maldives 960
            Mali 223
            Malta 356
            Marshall Islands 692
            Martinique 596
            Mauritania 222
            Mauritius 230
            Mayotte 262
            Mexico 52
            Micronesia, Federated States of 691
            Moldova (plus breakaway Transnistria) 373
            Monaco 377
            Mongolia 976
            Montenegro 382
            Montserrat 1 664
            Morocco 212
            Mozambique 258
            Myanmar 95
            Namibia 264
            Nauru 674
            Netherlands 31
            Netherlands Antilles 599
            Nepal 977
            New Caledonia 687
            New Zealand 64
            Nicaragua 505
            Niger 227
            Nigeria 234
            Niue 683
            Norfolk Island 6723
            North Korea 850
            Northern Ireland 44 28
            Northern Mariana Islands 1 670
            Norway 47
            Oman 968
            Pakistan 92
            Palau 680
            Panama 507
            Papua New Guinea 675
            Paraguay 595
            Peru 51
            Philippines 63
            Poland 48
            Portugal 351
            Puerto Rico 1 787 and 1 939
            Qatar 974
            Reunion 262
            Romania 40
            Russia 7
            Rwanda 250
            Saint-Barthélemy 590
            Saint Helena and Tristan da Cunha 290
            Saint Kitts and Nevis 1 869
            Saint Lucia 1 758
            Saint Martin (French side) 590
            Saint Pierre and Miquelon 508
            Saint Vincent and the Grenadines 1 784
            Samoa 685
            Sao Tome and Principe 239
            Saudi Arabia 966
            Senegal 221
            Serbia 381
            Seychelles 248
            Sierra Leone 232
            Sint Maarten (Dutch side) 1 721
            Singapore 65
            Slovakia 421
            Slovenia 386
            Solomon Islands 677
            Somalia 252
            South Africa 27
            South Korea 82
            South Sudan 211
            Spain 34
            Sri Lanka 94
            Sudan 249
            Suriname 597
            Swaziland 268
            Sweden 46
            Switzerland 41
            Syria 963
            Taiwan 886
            Tajikistan 992
            Tanzania 255
            Thailand 66
            Togo 228
            Tokelau 690
            Tonga 676
            Trinidad and Tobago 1 868
            Tunisia 216
            Turkey 90
            Turkmenistan 993
            Turks and Caicos Islands 1 649
            Tuvalu 688
            Uganda 256
            Ukraine 380
            United Arab Emirates 971
            United Kingdom 44
            United States of America 1
            Uruguay 598
            Uzbekistan 998
            Vanuatu 678
            Venezuela 58
            Vietnam 84
            U.S. Virgin Islands 1 340
            Wallis and Futuna 681
            West Bank 970
            Yemen 967
            Zambia 260
            Zimbabwe 263      



             */






        }
    }
}