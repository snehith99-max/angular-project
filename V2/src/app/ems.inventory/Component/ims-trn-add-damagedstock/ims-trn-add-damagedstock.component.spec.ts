import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnAddDamagedstockComponent } from './ims-trn-add-damagedstock.component';

describe('ImsTrnAddDamagedstockComponent', () => {
  let component: ImsTrnAddDamagedstockComponent;
  let fixture: ComponentFixture<ImsTrnAddDamagedstockComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnAddDamagedstockComponent]
    });
    fixture = TestBed.createComponent(ImsTrnAddDamagedstockComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
