import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LawTrnArbitrationaddComponent } from './law-trn-arbitrationadd.component';

describe('LawTrnArbitrationaddComponent', () => {
  let component: LawTrnArbitrationaddComponent;
  let fixture: ComponentFixture<LawTrnArbitrationaddComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LawTrnArbitrationaddComponent]
    });
    fixture = TestBed.createComponent(LawTrnArbitrationaddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
