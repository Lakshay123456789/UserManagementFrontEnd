using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Text;
using UserManagementFrontEnd.Models;

namespace UserManagementFrontEnd.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {

            List<User> users = new List<User>();
            using (var httpClient = new HttpClient())
            {

                using (var response = await httpClient.GetAsync("https://localhost:7234/api/User/GetAllUser"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        users = JsonConvert.DeserializeObject<List<User>>(apiResponse);
                    }

                }
            }
            return View(users);

        }

        [HttpGet]

        public IActionResult AddUser()
        {
            UserDto user = new UserDto();

            return View(user);
        }


        [HttpPost]

        public async Task<IActionResult> AddUser(UserDto model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    HttpClient client = new HttpClient();

                    StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                    using (var response = await client.PostAsync("https://localhost:7234/api/User/AddNewUser", content))
                    { 
                        if (response.IsSuccessStatusCode)
                        {
                            return Redirect("Index");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred: " + ex.Message);
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> DeleteUser(Guid id)
        {
            try
            {
           
                string apiUrl = $"https://localhost:7234/api/User/GetUserById?id={id}";

                using (HttpClient client = new HttpClient())
                {
               
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        var product = JsonConvert.DeserializeObject<User>(jsonResponse);

                        return View(product);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "API request failed. User not found.");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
            }

            return RedirectToAction("Error");
        }
        [HttpPost]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                string apiUrl = $"https://localhost:7234/api/User/DeleteUser?Id={id}";

                using (HttpClient client = new HttpClient())
                {
               
                    HttpResponseMessage response = await client.DeleteAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "API request failed. Product not found or could not be deleted.");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
            }

            return RedirectToAction("Error");
        }

        [HttpGet]
        public async Task<ActionResult> Edit(Guid id)
        {
           
            try
            {
                string apiUrl = $"https://localhost:7234/api/User/GetUserById?id={id}";

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        var user = JsonConvert.DeserializeObject<User>(jsonResponse);
                        var userDto = new UserDto
                        {
                            Id= user.Id,
                            FirstName=user.FirstName,
                            LastName=user.LastName,
                            Contact=user.Contact,
                            Email=user.Email

                        };
                        return View(userDto);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "API request failed. Product not found.");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
            }

            return RedirectToAction("Error");
        }
        [HttpPost]
        public async Task<ActionResult> Edit(UserDto userDto)
        {
            try
            {
               
                string apiUrl = "https://localhost:7234/api/User/UpdateUser";
              
               
                using (HttpClient client = new HttpClient())
                {
                   
                    var jsonContent = JsonConvert.SerializeObject(userDto);
                    var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PutAsync($"{apiUrl}?Id={userDto.Id}", httpContent);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", "User");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "API request failed. Please try again later.");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
            }

            return RedirectToAction("Error");
        }

    }
}
