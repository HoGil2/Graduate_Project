import numpy as np
import cv2

#base_image = cv2.imread("./img/1_1.jpg")
base_image = cv2.imread("./Edu_Img/inside_angle_shot/2.jpg")

def nothing(x):
    pass

# HSV Threading을 통한 이미지 변조 시도
def show_transform_windows():
    cv2.namedWindow("Tracking")
    # H,S,V의 최저, 최대 조절 트랙바 생성
    cv2.createTrackbar("LH", "Tracking", 0, 255, nothing)
    cv2.createTrackbar("LS", "Tracking", 0, 255, nothing)
    cv2.createTrackbar("LV", "Tracking", 0, 255, nothing)
    cv2.createTrackbar("UH", "Tracking", 255, 255, nothing)
    cv2.createTrackbar("US", "Tracking", 255, 255, nothing)
    cv2.createTrackbar("UV", "Tracking", 255, 255, nothing)

    while True:
        hsv = cv2.cvtColor(base_image, cv2.COLOR_BGR2HSV)
        # 트랙바 값 가져옴
        l_h = cv2.getTrackbarPos("LH", "Tracking")
        l_s = cv2.getTrackbarPos("LS", "Tracking")
        l_v = cv2.getTrackbarPos("LV", "Tracking")
        u_h = cv2.getTrackbarPos("UH", "Tracking")
        u_s = cv2.getTrackbarPos("US", "Tracking")
        u_v = cv2.getTrackbarPos("UV", "Tracking")

        l_b = np.array([l_h, l_s, l_v])
        u_b = np.array([u_h, u_s, u_v])
        # 필터링 된 마스크
        mask = cv2.inRange(hsv, l_b, u_b)

        res = cv2.bitwise_and(base_image, base_image, mask=mask)

        cv2.imshow("res", res)
        key = cv2.waitKey(1)
        if key == 27:
            break

# opencv의 HoughCircles를 이용한 원 디텍션
# cv2.HoughCircles(image, method, dp, minDist[, circles[, param1[, param2[, minRadius[, maxRadius]]]]])
#
# image : 입력 이미지로 그레이스케일 이미지이어야 함
# method : 원을 검출하는 방법
# dp : 이미지 해상도에 대한 accumulator 해상도의 비율의 역수
#    예를 들어 dp=1이면 배열 accumulator는 입력 이미지와 같은 해상도를 가진다.
# minDist : 검출된 원 사이의 최소 거리
# circles : 발견된 원에 대한 벡터, 각 벡터는 3개(x,y,radius) 또는 4개(x,y,radius,votes)의 원소를 가진다.
# param1 : 지정한 원 검출방법을 위한 파라미터.
#        HOUGH_GRADIENT의 경우 캐니 에지 디텍터의 높은 쓰레드쏠드 값. 낮은 쓰레드쏠드값은 0.5해서 사용하게 됨
# param2 : 지정한 원 검출방법을 위한 파라미터.
#        HOUGH_GRADIENT의 경우 accumulator 쓰레도쏠드 값이다. 이 값이 너무 작으면 거짓 원이 검출. 가장 큰 accumulator 값을 가지는 원이 먼저 리턴.
# minRadius : 검출하려는 원의 최소 반지름. 크기를 알 수 없는 경우 0으로 지정
# maxRadius : 검출하려는 원의 최대 반지름. 크기를 알 수 없는 경우 0으로 지정. 음수를 지정하면 원의 중심만 리턴
def show_circle_detection():
    image_gray = cv2.cvtColor(base_image, cv2.COLOR_BGR2GRAY)

    image_gaussian = cv2.GaussianBlur(image_gray, (3, 3), 0)

    circles = cv2.HoughCircles(image_gaussian, cv2.HOUGH_GRADIENT, dp=1, minDist=20,
                               param1=50, param2=35, minRadius=0, maxRadius=0)
    circles = np.uint16(np.around(circles))

    for c in circles[0,:]:
        center = (c[0], c[1])
        radius = c[2]

        # 바깥원
        cv2.circle(image_gaussian, center, radius, (0, 255, 0), 2)

        # 중심원
        cv2.circle(image_gaussian, center, 2, (0, 0, 255), 3)

    cv2.imshow("Detection", image_gaussian)
    cv2.waitKey(0)
    cv2.destroyAllWindows()

# https://opencv-python.readthedocs.io/en/latest/doc/15.imageContours/imageContours.html
# http://117.52.129.49/helloworld/8344782
def image_global_thresh_contours():
    img_gray = cv2.cvtColor(base_image, cv2.COLOR_BGR2GRAY)

    img_gaussian = cv2.GaussianBlur(img_gray, (3, 3), 0)
    kernel = np.ones((5, 5), np.uint8)
    img_gradient = cv2.morphologyEx(img_gray, cv2.MORPH_GRADIENT, kernel)

    ret, thresh = cv2.threshold(img_gaussian, 127, 255, 0)

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
    cv2.imshow("Contours1", thresh)

    kernel = np.ones((9, 9), np.uint8)
    closing1 = cv2.morphologyEx(thresh, cv2.MORPH_CLOSE, kernel)

    cv2.imshow("closing1", closing1)

    image, contours, hierachy = cv2.findContours(closing1, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)
    image = cv2.drawContours(base_image, contours, -1, (0, 255, 0), 3)

    cv2.imshow("Contours", image)
    cv2.waitKey(0)
    cv2.destroyAllWindows()


# https://opencv-python.readthedocs.io/en/latest/doc/15.imageContours/imageContours.html
# http://117.52.129.49/helloworld/8344782
def image_adaptive_one_contours():
    img_gray = cv2.cvtColor(base_image, cv2.COLOR_BGR2GRAY)

    img_gaussian = cv2.GaussianBlur(img_gray, (3, 3), 0)
    kernel = np.ones((5, 5), np.uint8)
    img_gradient = cv2.morphologyEx(img_gray, cv2.MORPH_GRADIENT, kernel)

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
    adaptive_thresh1 = cv2.adaptiveThreshold(img_gaussian, 255, cv2.ADAPTIVE_THRESH_GAUSSIAN_C, cv2.THRESH_BINARY, 11, 2)
    cv2.imshow("Contours2", adaptive_thresh1)

    kernel = np.ones((9, 9), np.uint8)
    closing2 = cv2.morphologyEx(adaptive_thresh1, cv2.MORPH_CLOSE, kernel)

    cv2.imshow("closing2", closing2)

    image2, contours2, hierachy3 = cv2.findContours(closing2, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)
    image2 = cv2.drawContours(base_image, contours2, -1, (0, 255, 0), 3)

    cv2.imshow("Contours4", image2)
    cv2.waitKey(0)
    cv2.destroyAllWindows()


# https://opencv-python.readthedocs.io/en/latest/doc/15.imageContours/imageContours.html
# http://117.52.129.49/helloworld/8344782
def image_preprocessing():
    img_gray = cv2.cvtColor(base_image, cv2.COLOR_BGR2GRAY)

    # bilateral filter
    # https://datascienceschool.net/view-notebook/c4121d311aa34e6faa84f62ef06e43b0/
    # 가우시안 필터링을 쓰면 이미지의 경계선도 흐려지기 때문에 양방향 필터링(Bilateral Filtering) 사용
    # 양방향 필터링은 두 픽셀과의 거리 뿐 아니라 두 픽셀의 명암값의 차이도 커널에 넣어서 가중치로 곱한다.
    # 따라서 픽셀값의 차이가 너무 가중치가 0에 가까운 값이 되어 합쳐지지 않으므로 영역과
    # 영역사이의 경계선이 잘 보존된다.
    # bilateralFilter(src, d, sigmaColor, sigmaSpace)
    # src : 원본 이미지
    # d : 커널 크기
    # sigmaColor : 색공간 표준편차. 값이 크면 색이 많이 달라도 픽셀들이 서로 영향을 미친다.
    # sigmaSpace : 거리공간 표준편차. 값이 크면 멀리 떨어져있는 픽셀들이 서로 영향을 미친다.
    img_bilateral = cv2.bilateralFilter(img_gray, 9, 75, 75)
    cv2.imshow("Bilateral", img_bilateral)
    cv2.waitKey(0)
    cv2.destroyAllWindows()
    #kernel = np.ones((5, 5), np.uint8)
    #img_gradient = cv2.morphologyEx(img_gaussian, cv2.MORPH_GRADIENT, kernel)

    #ret, thresh = cv2.threshold(img_gradient, 127, 255, 0)

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
    adaptive_thresh2 = cv2.adaptiveThreshold(img_bilateral, 255, cv2.ADAPTIVE_THRESH_MEAN_C, cv2.THRESH_BINARY, 11, 2)
    cv2.imshow("Contours3", adaptive_thresh2)

    kernel = np.ones((11, 11), np.uint8)
    closing3 = cv2.morphologyEx(adaptive_thresh2, cv2.MORPH_CLOSE, kernel)

    cv2.imshow("closing3", closing3)

    image3, contours3, hierachy3 = cv2.findContours(closing3, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_NONE)
    image3 = cv2.drawContours(base_image, contours3, -1, (0, 255, 0), 3)

    cv2.imshow("image3", image3)

if __name__ == "__main__":
    #show_circle_detection()
    image_preprocessing()