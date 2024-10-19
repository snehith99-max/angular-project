import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnDirectissuematerialComponent } from './ims-trn-directissuematerial.component';

describe('ImsTrnDirectissuematerialComponent', () => {
  let component: ImsTrnDirectissuematerialComponent;
  let fixture: ComponentFixture<ImsTrnDirectissuematerialComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnDirectissuematerialComponent]
    });
    fixture = TestBed.createComponent(ImsTrnDirectissuematerialComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
