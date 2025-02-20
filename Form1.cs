using Microsoft.VisualBasic.Devices;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace A2

{
    public partial class Form1 : Form
    {
        //initializing image_id as a string
        private string image_Id = "";
        private List<Artwork>? artworksList = new List<Artwork>(); //list to hold Details
        public Form1()
        {
            InitializeComponent();
            LoadArtworks(); //method to get atwork from API
        }
        private async void LoadArtworks()
        {

            string uri = "https://api.artic.edu/api/v1/artworks?fields=id,title,artist_title,image_id,date_display,thumbnail,medium_display&page=1&limit=30";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    //added because it wasn't allowing access to the API
                   client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3");
                    HttpResponseMessage response = await client.GetAsync(uri);
                    //ensure HTTP response is successful
                    response.EnsureSuccessStatusCode();
                    //desrialize JSON
                    string json = await response.Content.ReadAsStringAsync();
                    //pptions to configure JSON deserialization
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    // Deserializes the JSON response into an ArtworkResponse 
                    var data = JsonSerializer.Deserialize<ArtworkResponse>(json, options);

                    //If the API response contains artwork data, store it in the list and update the UI
                    if (data?.Data != null)
                    {
                        artworksList = data.Data; //Store the artwork data in a list(so API is not called repeatedly)
                        PopulateListView(); // Populate the List View with artwork data
                    }
                }
            }
            //error message if something goes wrong
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading artwork data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    private void PopulateListView()
    { 
      listViewBox.Items.Clear();  // Clear the list view

    foreach (var artwork in artworksList) //loop through artworks in artworksList
    {
       //Create a new ListViewItem
        ListViewItem item = new ListViewItem(artwork.Title);
        //Add additional details as subitems 
        item.SubItems.Add(artwork.ArtistTitle);
        item.SubItems.Add(artwork.DateDisplay);
        item.Tag = artwork.ImageId;  // Store only the imageId in the Tag property
        listViewBox.Items.Add(item);
    }
}

static async Task<System.IO.Stream>  GetArtAsync(HttpClient clientToCall) //string imageId)
        {
            HttpClient artDataClient = new HttpClient();
            Uri artDataUri = new Uri("https://api.artic.edu/api/v1/artworks?fields=id,title,artist_title,image_id,date_display,thumbnail,medium_display?page=1&limit=30");
            artDataClient.BaseAddress = artDataUri;

            artDataClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            System.IO.Stream httpMethodResult = GetArtAsync(artDataClient).Result;

            System.IO.Stream artResult = Stream.Null;

            string endpoint = "https://www.artic.edu/iiif/2/{image_id}/full/843,/0/default.jpg\r\n"; ;
            HttpResponseMessage artHttpResponse = await clientToCall.GetAsync(endpoint);

           // successful call
   if (artHttpResponse.IsSuccessStatusCode)
            {
                artResult = await artHttpResponse.Content.ReadAsStreamAsync();

            }
            return artResult;



            //if (string.IsNullOrEmpty(imageId))
            //    return Stream.Null;


            //string endpoint = $"https://www.artic.edu/iiif/2/{imageId}/full/843,/0/default.jpg";


            //HttpResponseMessage response = await clientToCall.GetAsync(endpoint);


            //return response.IsSuccessStatusCode ? await response.Content.ReadAsStreamAsync() : Stream.Null;
        }

        private void listViewBox_SelectedIndexChanged(object sender, EventArgs e)
        {
              // Check if at least one item is selected in the ListView
            if (listViewBox.SelectedItems.Count > 0)
            {
                //get the Tag property of the first selected item
                var selectedTag = listViewBox.SelectedItems[0].Tag;
                Console.WriteLine($"Tag value: {selectedTag}"); // Debug message
                //Ensure the tag is not null before using it
                if (selectedTag != null)
                {
                    image_Id = selectedTag.ToString();
                    Console.WriteLine($"Selected Image ID: {image_Id}");
                }
                else
                {
                    Console.WriteLine("Tag is null!");
                }
            }
        }

        //made async to be able to use await
        private async void detailsBttn_Click(object sender, EventArgs e)
        {
            Console.WriteLine($"Selected Image ID: {image_Id}"); // Debugging
            //Check if an artwork has been selected 

            if (string.IsNullOrEmpty(image_Id))
            {
                MessageBox.Show("Please select an artwork first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        
        // get artwork details from API
        string detailsUri = $"https://api.artic.edu/api/v1/artworks/{image_Id}" +
                $"?fields=id,title,artist_title,image_id,date_display,thumbnail,medium_display";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(detailsUri);
                    response.EnsureSuccessStatusCode();

                    string json = await response.Content.ReadAsStringAsync();

                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var data = JsonSerializer.Deserialize<ArtworkResponse>(json, options);

                    if (data != null && data.Data != null && data.Data.Count > 0)
                    {
                        Artwork artwork = data.Data[0]; // Get the first artwork
                        //?? used to take into account if there are null values in the API
                        //and an empty string will be shown if it is null
                        string details = $"Title: {artwork.Title ?? ""}\n" +
                 $"Artist: {artwork.ArtistTitle ?? ""}\n" +
                 $"Date: {artwork.DateDisplay ?? ""}\n" +
                 $"Medium: {artwork.MediumDisplay ?? ""}";
                        //art details are shown in message box
                        MessageBox.Show(details, "Artwork Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        //if there is no details
                        MessageBox.Show("No details found for this artwork.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching artwork details: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void viewBttn_Click(object sender, EventArgs e)
        {

        }
    }
    //corresponding fields
    //? to cover if the fields are null in the API
    public class Artwork
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? ArtistTitle { get; set; }
        public string? ImageId { get; set; }
        public string? DateDisplay { get; set; }
        public Thumbnail? Thumbnail { get; set; }
        public string? MediumDisplay { get; set; }
    }
    //Thumbnail has it's own values within the data
    //? to cover if the fields are null in the API
    public class Thumbnail
    {
        public string? Lqip { get; set; } 
        public int? Width { get; set; }
        public int? Height { get; set; }
        public string? AltText { get; set; }
    }

    public class ArtworkResponse
    {
        //// The 'Data' property contains a list of 'Artwork' objects. 
        //? means it can be null if no data in the API
        public List<Artwork>? Data { get; set; }
    }






}

