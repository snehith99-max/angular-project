import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnIssuematerialViewComponent } from './ims-trn-issuematerial-view.component';

describe('ImsTrnIssuematerialViewComponent', () => {
  let component: ImsTrnIssuematerialViewComponent;
  let fixture: ComponentFixture<ImsTrnIssuematerialViewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnIssuematerialViewComponent]
    });
    fixture = TestBed.createComponent(ImsTrnIssuematerialViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
