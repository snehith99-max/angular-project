import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnViewdcComponent } from './ims-trn-viewdc.component';

describe('ImsTrnViewdcComponent', () => {
  let component: ImsTrnViewdcComponent;
  let fixture: ComponentFixture<ImsTrnViewdcComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnViewdcComponent]
    });
    fixture = TestBed.createComponent(ImsTrnViewdcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
