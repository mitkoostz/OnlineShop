using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Dtos.ProductReviews;
using Api.Errors;
using Api.Extensions;
using Core.Entities;
using Core.Entities.Identity;
using Core.Entities.Reviews;
using Core.Interfaces;
using Core.Specifications.ProductReviews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class ProductReviewController : BaseApiController
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IUnitOfWork unitOfWork;
        public ProductReviewController(
            UserManager<AppUser> userManager,
            IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }


        [Authorize]
        [HttpPost]
        public async Task<ActionResult> SubmitReview(ProductReviewDto productReview)
        {
            AppUser user = await userManager.FindByEmailFromClaimsPrinciple(HttpContext.User);
            bool hasComment = !string.IsNullOrEmpty(productReview.Comment);
            Product product = await unitOfWork.Repository<Product>().GetByIdAsync(productReview.ProductId);
            if(product == null){
                return BadRequest(new ApiResponse(400,"The product you are submiting review doesn't exists!"));
            }
            IReadOnlyList<ProductReview> currentReviews = await unitOfWork.Repository<ProductReview>()
                 .ListAsync(new ProductReviewFindByProductId(product.Id));
            bool userAlreadyHasSubmuitReview = false;
            int sumOfAllReviewRating = 0;   

            foreach (var currentReview in currentReviews)
            {
                sumOfAllReviewRating += currentReview.StarRate;
                if(currentReview.UserId == user.Id)
                {
                    //User already has submitted review
                    userAlreadyHasSubmuitReview = true;
                }
            }

            if(userAlreadyHasSubmuitReview)
            {
                return BadRequest(new ApiResponse(400, "You have already submitted review!"));
            }


            ProductReview review = new ProductReview()
            {
                UserId = user.Id,
                UserEmail = user.Email,
                Product = product,
                UserName = user.DisplayName,
                Date = DateTime.Now,
                StarRate = productReview.StarRating,
                HasComment = hasComment,
                Comment = productReview.Comment
            };
            //we calculate avarage product review by adding this review also
            decimal productAvarageRating = (sumOfAllReviewRating + review.StarRate) / (currentReviews.Count + 1);
            product.AverageReviewRate = productAvarageRating;

            unitOfWork.Repository<ProductReview>().Add(review);
            unitOfWork.Repository<Product>().Update(product);


            await unitOfWork.Complete();
            return Ok(new ApiResponse(200,"Thank you for submiting review!"));
        }
    }
}