import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PayMstIncometaxratesComponent } from './pay-mst-incometaxrates.component';

describe('PayMstIncometaxratesComponent', () => {
  let component: PayMstIncometaxratesComponent;
  let fixture: ComponentFixture<PayMstIncometaxratesComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PayMstIncometaxratesComponent]
    });
    fixture = TestBed.createComponent(PayMstIncometaxratesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
