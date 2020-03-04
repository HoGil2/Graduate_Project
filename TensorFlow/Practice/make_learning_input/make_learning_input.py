import numpy as np
import cv2
import glob

DEBUG = False

file_path = './Edu_Img/inside_angle_shot'
base_images = glob.glob(file_path + '/*.jpg')

#base_image = cv2.imread("./Edu_Img/unprocessing/1_inside_angle_shot/y/13.jpg")
base_image = cv2.imread("./Edu_Img/unprocessing/1_inside_angle_shot/4.jpg")

white_ball_stroked = False

white_lower = np.array([160, 220, 220], dtype=np.uint8)
white_upper = np.array([255, 255, 255], dtype=np.uint8)
yellow_lower = np.array([0, 170, 240], dtype=np.uint8)
yellow_upper = np.array([40, 230, 255], dtype=np.uint8)

red_lower = np.array([0, 0, 210], dtype=np.uint8)
red_upper = np.array([50, 50, 255], dtype=np.uint8)

# https://opencv-python.readthedocs.io/en/latest/doc/15.imageContours/imageContours.html
# http://117.52.129.49/helloworld/8344782
def image_preprocessing():
    img_gray = cv2.cvtColor(base_image, cv2.COLOR_BGR2GRAY)

    kernel = np.ones((3, 3), np.uint8)
    # 경계 탐색을 위한 MORPH_GRADIENT
    img_gradient = cv2.morphologyEx(img_gray, cv2.MORPH_GRADIENT, kernel)
    # 잡음 제거를 위한 AdaptiveThreshold
    # cv2.adaptiveThreshold(img, value, adaptivemethod, thresholdType, blocksize, C)
    # global Threshold는 문턱 값을 하나의 이미지 전체에 적용시키는 반면 adaptive Threshold는 이미지의 구역구역마다 threshold를 실행 시켜주는 것이다
    # img: grayscale 이미지
    # value: adaptivemethod에 의해 계산된 문턱 값과 thresholdType에 의해 픽셀에 적용될 최대 값
    # adaptive method: 사용할 문턱값 계산 알고리즘
    # cv2.ADAPTIVE_THRESH_GAUSSIAN_C: X, Y를 중심으로 block Size * block Size 안에 있는 픽셀 값의 평균에서 C를 뺸 값을 문턱값으로 함
    # cv2.ADAPTIVE_THRESH_MEAN_C: X, Y를 중심으로 block Size * block Size 안에 있는 Gaussian 윈도우 기반 가중치들의 합에서 C를 뺀 값을 문턱값으로 한다.
    # blocksize: block * block size에 각각 다른 문턱값이 적용된다.
    # C: 보정 상수로서 adaptive에 계산된 값에서 양수면 빼주고 음수면 더해준다.
    # 출처: https://hoony-gunputer.tistory.com/99?category=753147 [후니의 컴퓨터]
    adaptive_thresh2 = cv2.adaptiveThreshold(img_gradient, 255, cv2.ADAPTIVE_THRESH_MEAN_C, cv2.THRESH_BINARY_INV, 11, 2)

    if DEBUG is True:
        img_copy = base_image.copy()
        circles = cv2.HoughCircles(img_gradient, cv2.HOUGH_GRADIENT, dp=1, minDist=5,
                                  param1=50, param2=20, minRadius=5, maxRadius=20)
        #circles = cv2.HoughCircles(adaptive_thresh2, cv2.HOUGH_GRADIENT, dp=1, minDist=7,
        #                           param1=20, param2=20, minRadius=5, maxRadius=30)

        if circles is not None:
            circles = np.uint16(np.around(circles))

            for c in circles[0, :]:
                center = (c[0], c[1])
                radius = c[2]

                # 바깥원
                cv2.circle(img_copy, center, radius, (0, 255, 0), 2)

                # 중심원
                cv2.circle(img_copy, center, 2, (0, 0, 255), 3)
        else:
            print("No Circles!")

        cv2.imshow("gradient", img_gradient)
        cv2.imshow("adaptive_thresh2", adaptive_thresh2)
        cv2.imshow("Detection", img_copy)
        cv2.waitKey(0)
        cv2.destroyAllWindows()

    img_copy = base_image.copy()
    kernel = np.ones((1, 1), np.uint8)
    # 작은 틈을 메우고 경계를 강화하는 Closing
    closing3 = cv2.morphologyEx(adaptive_thresh2, cv2.MORPH_CLOSE, kernel)
    # 원 탐색
    circles = cv2.HoughCircles(closing3, cv2.HOUGH_GRADIENT, dp=1, minDist=5,
                              param1=50, param2=20, minRadius=5, maxRadius=20)

    if circles is not None:
        circles = np.uint16(np.around(circles))
        # 원의 중심 픽셀값을 확인하기 위한 배열 생성
        i = circles[0].__len__()
        selected_colors = np.zeros((50, i*50, 3), dtype=np.uint8)
        # 학습 이미지 생성
        processed_img = np.zeros((32, 64, 3), np.uint8)
        height, width, channel = img_copy.shape

        # 타구를 다른 색으로 그리기 위함
        if white_ball_stroked == True:
            masked_img = cv2.inRange(img_copy, white_lower, white_upper)
        else:
            masked_img = cv2.inRange(img_copy, yellow_lower, yellow_upper)

        # 찾은 원들의 중심점을 이용하여 학습이미지로의 변환
        for c in circles[0, :]:
            center = (c[0], c[1])
            radius = c[2]
            # 원의 중심점
            # print(center[1], center[0])
            # 학습 이미지에서로 변환된 중심점
            # print(round(center[1]*(32/height)), round(center[0]*(64/width)))
            processed_height = round(center[1] * (32 / height))
            processed_width = round(center[0] * (64 / width))

            print(masked_img[center[1], center[0]])
            print(img_copy[center[1], center[0]])

            # 중심점 pixel의 value가 255라면 masked 씌운 해당 색이므로 타구
            # 해당 공을 다른 색으로 학습이미지에 그림
            if masked_img[center[1], center[0]] == 255:
                cv2.circle(processed_img, (int(processed_width), int(processed_height)), 1, (255, 255, 255), -1)
            else:
                cv2.circle(processed_img, (int(processed_width), int(processed_height)), 1, (0, 0, 255), -1)
            selected_colors[:, (i-1)*50:i*50] = img_copy[center[1], center[0]]
            i -= 1
            # 바깥원
            cv2.circle(img_copy, center, radius, (0, 255, 0), 2)

            # 중심원
            cv2.circle(img_copy, center, 2, (0, 0, 255), 3)

        cv2.imshow("Selected Colors", selected_colors)
    else:
        print("No Circles!")

    if DEBUG is True:
        cv2.imshow("closing3", closing3)
        cv2.imshow("Masked_Img", masked_img)
    cv2.imshow("Processed_Img", processed_img)
    cv2.imshow("Detection", img_copy)
    cv2.waitKey(0)
    cv2.destroyAllWindows()

if __name__ == "__main__":
    image_preprocessing()
