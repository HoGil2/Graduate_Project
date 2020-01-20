import numpy as np
import cv2

base_image = cv2.imread("./img/1_1.jpg")

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

    circles = cv2.HoughCircles(image_gray, cv2.HOUGH_GRADIENT, dp=1, minDist=20,
                               param1=50, param2=35, minRadius=0, maxRadius=0)
    circles = np.uint16(np.around(circles))

    for c in circles[0,:]:
        center = (c[0], c[1])
        radius = c[2]

        # 바깥원
        cv2.circle(image_gray, center, radius, (0, 255, 0), 2)

        # 중심원
        cv2.circle(image_gray, center, 2, (0, 0, 255), 3)

    cv2.imshow("Detection", image_gray)
    cv2.waitKey(0)
    cv2.destroyAllWindows()

show_circle_detection()