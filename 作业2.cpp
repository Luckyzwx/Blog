#include <iostream>
using namespace std;

#include <stdio.h>
#include <stdlib.h>
#include <time.h>

#define random(x) (rand()%x)

int main()
{
int i, a, b;
int mode = 0,result=0;//0:�� 1:�� 2:�� 3:��
for (i = 0; i<30; i++) //�������n����Ŀ
{
a = random(100); //����һ��0~99֮��������
b = random(100); //����һ��0~99֮��������
mode = random(4); //����һ��0~3֮�������������������
cout<< a; //��ӡ��ʽ
switch (mode) //ȷ�������
{
case 0:
cout<<"+";
result = a + b;
break;
case 1:
cout<<"-";
result = a - b;
break;
case 2:
cout<<"*";
result = a * b;
break;
case 3:
cout<<"/";
result = a / b;
break;
default:
cout<<"somethingis wrong!\n";
break;
}
cout<< b<<"="<<endl;

}
return 0;
}
