import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PayMstBankmasterComponent } from './pay-mst-bankmaster.component';

describe('PayMstBankmasterComponent', () => {
  let component: PayMstBankmasterComponent;
  let fixture: ComponentFixture<PayMstBankmasterComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PayMstBankmasterComponent]
    });
    fixture = TestBed.createComponent(PayMstBankmasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
